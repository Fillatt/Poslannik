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

        // Проверяем, был ли это админ
        if (chat.AdminId == userId)
        {
            // Получаем оставшихся участников
            var remainingParticipants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
            var participantsList = remainingParticipants.ToList();

            if (participantsList.Any())
            {
                // Передаем права первому оставшемуся участнику
                var newAdminId = participantsList.First().UserId;
                await TransferAdminRightsAsync(chatId, newAdminId);
            }
            else
            {
                // Если участников не осталось, удаляем чат
                await DeleteChatAsync(chatId);
                return;
            }
        }

        // Уведомляем всех участников об удалении
        await NotifyParticipantRemovedAsync(chatId, userId);
    }

    /// <summary>
    /// Передает права администратора другому участнику
    /// </summary>
    public async Task TransferAdminRightsAsync(Guid chatId, Guid newAdminId)
    {
        var chats = await _chatRepository.GetAllAsync();
        var chat = chats.FirstOrDefault(c => c.Id == chatId);

        if (chat == null)
            return;

        var updatedChat = chat with { AdminId = newAdminId };
        await _chatRepository.UpdateAsync(updatedChat);

        // Уведомляем всех участников о смене администратора
        await NotifyAdminRightsTransferredAsync(chatId, newAdminId);
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
    public async Task DeleteChatAsync(Guid chatId)
    {
        // Сначала получаем всех участников для уведомления
        var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);
        var participantsList = participants.ToList();

        // Удаляем всех участников чата
        foreach (var participant in participantsList)
        {
            await _chatParticipantRepository.DeleteByUserAndChatAsync(chatId, participant.UserId);
        }

        // Удаляем сам чат
        await _chatRepository.DeleteAsync((long)(object)chatId);

        // Уведомляем участников об удалении
        await NotifyDeleteAsync(chatId);
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
    private async Task NotifyDeleteAsync(Guid chatId)
    {
        var participants = await _chatParticipantRepository.GetAllAsync();
        var chatParticipants = participants.Where(p => p.ChatId == chatId).ToList();

        foreach (var participant in chatParticipants)
        {
            await Clients.User(participant.UserId.ToString())
                .SendAsync(HubConstants.ChatEvents.ChatDeleted, chatId);
        }
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

        foreach(var participant in participants)
        {
            // Уведомляем также удаленного пользователя
            await Clients.User(participant.UserId.ToString())
                .SendAsync(HubConstants.ChatEvents.ParticipantRemoved, chatId, removedUserId);
        }
    }

    /// <summary>
    /// Уведомляет участников о передаче прав администратора
    /// </summary>
    private async Task NotifyAdminRightsTransferredAsync(Guid chatId, Guid newAdminId)
    {
        var participants = await _chatParticipantRepository.GetByChatIdAsync(chatId);

        foreach (var participant in participants)
        {
            await Clients.User(participant.UserId.ToString())
                .SendAsync(HubConstants.ChatEvents.AdminRightsTransferred, chatId, newAdminId);
        }
    }
}
