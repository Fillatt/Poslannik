using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Hubs;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Api.Hubs;

/// <summary>
/// SignalR хаб для управления чатами в реальном времени
/// </summary>
[Authorize]
public class ChatHub : Hub, IChatHub
{
    private readonly IChatRepository _chatRepository;
    private readonly IChatParticipantRepository _chatParticipantRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public ChatHub(
        IChatRepository chatRepository,
        IChatParticipantRepository chatParticipantRepository,
        IMessageRepository messageRepository,
        IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _chatParticipantRepository = chatParticipantRepository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Получает список участников чата
    /// </summary>
    public async Task<IEnumerable<ChatParticipant>> GetChatParticipantsAsync(Guid chatId)
    {
        return await _chatParticipantRepository.GetByChatIdAsync(chatId);
    }

    /// <summary>
    /// Удаляет участника из чата
    /// </summary>
    public async Task RemoveParticipantAsync(Guid chatId, Guid userId)
    {
        // Получаем чат для проверки прав
        var chats = await _chatRepository.GetAllAsync();
        var chat = chats.FirstOrDefault(c => c.Id == chatId);

        if (chat == null)
            return;

        // Получаем информацию об удаляемом пользователе для системного сообщения
        var removedUser = await _userRepository.GetUserByIdAsync(userId);
        var userName = removedUser?.UserName ?? removedUser?.Login ?? "Пользователь";

        // Удаляем участника
        await _chatParticipantRepository.DeleteByUserAndChatAsync(chatId, userId);

        // Уведомляем всех участников об удалении
        await NotifyParticipantRemovedAsync(chatId, userId);
    }

    /// <summary>
    /// Получает все чаты пользователя
    /// </summary>
    public async Task<IEnumerable<Chat>> GetUserChatsAsync(Guid userId)
    {
        return await _chatRepository.GetChatsByUserIdAsync(userId);
    }

    /// <summary>
    /// Создает новый чат и уведомляет всех участников
    /// </summary>
    public async Task<Chat> CreateChatAsync(Chat chat, IEnumerable<Guid>? participantUserIds = null)
    {
        // Создаем новый чат с уникальным идентификатором
        var newChat = chat with { Id = Guid.NewGuid() };

        // Для группового чата с участниками используем специальный метод
        if (newChat.ChatType == ChatType.Group && participantUserIds != null && participantUserIds.Any())
        {
            await _chatRepository.AddChatWithParticipantsAsync(newChat, participantUserIds);
        }
        else
        {
            // Для личных чатов используем стандартный метод
            await _chatRepository.AddAsync(newChat);
        }

        // Уведомляем всех участников о создании чата
        await NotifyChatParticipantsAsync(newChat);

        return newChat;
    }

    /// <summary>
    /// Уведомляет всех участников чата о событии
    /// </summary>
    public async Task NotifyChatParticipantsAsync(Chat chat)
    {
        // Для личного чата уведомляем обоих пользователей
        if (chat.ChatType == ChatType.Private && chat.User1Id.HasValue && chat.User2Id.HasValue)
        {
            await Clients.User(chat.User1Id.Value.ToString())
                .SendAsync(HubConstants.ChatEvents.ChatCreated, chat);
            await Clients.User(chat.User2Id.Value.ToString())
                .SendAsync(HubConstants.ChatEvents.ChatCreated, chat);
        }
        // Для группового чата уведомляем всех участников
        else if (chat.ChatType == ChatType.Group)
        {
            var participants = await _chatParticipantRepository.GetAllAsync();
            var chatParticipants = participants.Where(p => p.ChatId == chat.Id).ToList();

            foreach (var participant in chatParticipants)
            {
                await Clients.User(participant.UserId.ToString())
                    .SendAsync(HubConstants.ChatEvents.ChatCreated, chat);
            }
        }
    }

    /// <summary>
    /// Обновляет информацию о чате и уведомляет участников
    /// </summary>
    public async Task UpdateChatAsync(Chat chat)
    {
        await _chatRepository.UpdateAsync(chat);

        // Уведомляем участников об обновлении
        await NotifyUpdateAsync(chat);
    }

    /// <summary>
    /// Удаляет чат и уведомляет участников
    /// </summary>
    public async Task DeleteChatAsync(Chat chat)
    {
        var chatId = chat.Id;
        if (chat.ChatType == ChatType.Group)
        {
            var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
            var participantsList = participants.ToList();

            // Удаляем всех участников чата
            foreach (var participant in participantsList)
            {
                await _chatParticipantRepository.DeleteByUserAndChatAsync(chatId, participant.UserId);
            }

            // Удаляем сам чат
            await _chatRepository.DeleteAsync(chatId);

            // Уведомляем участников об удалении
            await NotifyGroupChatDeleteAsync(chatId, participants);
        }
        else
        {
            await _chatRepository.DeleteAsync(chatId);
            await NotifyPrivateChatDeleteAsync(chat);
        }
    }

    /// <summary>
    /// Уведомляет участников об обновлении чата
    /// </summary>
    private async Task NotifyUpdateAsync(Chat chat)
    {
        var participants = await _chatParticipantRepository.GetAllAsync();
        var chatParticipants = participants.Where(p => p.ChatId == chat.Id).ToList();

        foreach (var participant in chatParticipants)
        {
            await Clients.User(participant.UserId.ToString())
                .SendAsync(HubConstants.ChatEvents.ChatUpdated, chat);
        }
    }

    /// <summary>
    /// Уведомляет участников об удалении чата
    /// </summary>
    private async Task NotifyGroupChatDeleteAsync(Guid chatId, IEnumerable<ChatParticipant> chatParticipants)
    {
        foreach (var participant in chatParticipants)
        {
            await Clients.User(participant.UserId.ToString())
                .SendAsync(HubConstants.ChatEvents.ChatDeleted, chatId);
        }
    }

    private async Task NotifyPrivateChatDeleteAsync(Chat chat)
    {
        await Clients.User(chat.User1Id.ToString())
               .SendAsync(HubConstants.ChatEvents.ChatDeleted, chat.Id);

        await Clients.User(chat.User2Id.ToString())
              .SendAsync(HubConstants.ChatEvents.ChatDeleted, chat.Id);
    }

    /// <summary>
    /// Вызывается при подключении клиента к хабу
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Вызывается при отключении клиента от хаба
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Уведомляет участников об удалении участника из чата
    /// </summary>
    private async Task NotifyParticipantRemovedAsync(Guid chatId, Guid removedUserId)
    {
        var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);

        foreach (var participant in participants)
        {
            await Clients.User(participant.UserId.ToString())
                .SendAsync(HubConstants.ChatEvents.ParticipantRemoved, chatId, removedUserId);
        }

        // Уведомляем также удаленного пользователя
        await Clients.User(removedUserId.ToString())
            .SendAsync(HubConstants.ChatEvents.ParticipantRemoved, chatId, removedUserId);
        
    }

    /// <summary>
    /// Добавляет участников в существующий групповой чат
    /// </summary>
    public async Task AddParticipantsAsync(Guid chatId, IEnumerable<Guid> participantUserIds)
    {
        // Получаем чат для проверки
        var chats = await _chatRepository.GetAllAsync();
        var chat = chats.FirstOrDefault(c => c.Id == chatId);

        if (chat == null || chat.ChatType != ChatType.Group)
            return;

        // Получаем текущих участников чата
        var existingParticipants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
        var existingUserIds = existingParticipants.Select(p => p.UserId).ToHashSet();

        // Добавляем новых участников
        foreach (var userId in participantUserIds)
        {
            // Проверяем, что участник еще не добавлен
            if (!existingUserIds.Contains(userId))
            {
                var participant = new ChatParticipant
                {
                    Id = Guid.NewGuid(),
                    ChatId = chatId,
                    UserId = userId
                };

                await _chatParticipantRepository.AddAsync(participant);
            }
        }

        // Уведомляем всех участников (старых и новых) о создании чата для новых пользователей
        await NotifyChatParticipantsAsync(chat);
    }
}
