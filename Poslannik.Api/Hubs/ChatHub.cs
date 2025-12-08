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
public class ChatHub : Hub, IChatHubRepository
{
    private readonly IChatRepository _chatRepository;
    private readonly IChatParticipantRepository _chatParticipantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;

    public ChatHub(
        IChatRepository chatRepository,
        IChatParticipantRepository chatParticipantRepository,
        IUserRepository userRepository,
        IMessageRepository messageRepository)
    {
        _chatRepository = chatRepository;
        _chatParticipantRepository = chatParticipantRepository;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
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
    /// Удаляет чат и уведомляет участников (только для админа)
    /// </summary>
    public async Task DeleteChatAsync(Guid chatId)
    {
        var currentUserId = Guid.Parse(Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
        var chat = await _chatRepository.GetByIdAsync(chatId);

        if (chat == null)
            return;

        // Проверяем, что пользователь - админ группы
        if (chat.ChatType == ChatType.Group && chat.AdminId != currentUserId)
            throw new UnauthorizedAccessException("Только администратор может удалить групповой чат");

        await _chatRepository.DeleteAsync((long)(object)chatId);

        // Уведомляем участников об удалении
        await NotifyDeleteAsync(chatId);
    }

    /// <summary>
    /// Выход пользователя из группового чата
    /// </summary>
    public async Task LeaveChatAsync(Guid chatId)
    {
        var currentUserId = Guid.Parse(Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
        var chat = await _chatRepository.GetByIdAsync(chatId);

        if (chat == null || chat.ChatType != ChatType.Group)
            return;

        // Получаем данные пользователя для системного сообщения
        var user = await _userRepository.GetUserByIdAsync(currentUserId);
        var displayName = user?.UserName ?? user?.Login ?? "Пользователь";

        // Получаем всех участников перед удалением
        var allParticipants = await _chatParticipantRepository.GetAllAsync();
        var remainingParticipants = allParticipants
            .Where(p => p.ChatId == chatId && p.UserId != currentUserId)
            .ToList();

        // Удаляем участника из чата
        var participant = allParticipants.FirstOrDefault(p => p.ChatId == chatId && p.UserId == currentUserId);
        if (participant != null)
        {
            await _chatParticipantRepository.DeleteAsync((long)(object)participant.Id);
        }

        // Создаем системное сообщение о выходе
        var systemMessage = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            SenderId = currentUserId,
            EncryptedMessage = System.Text.Encoding.UTF8.GetBytes($"{displayName} покинул чат"),
            SentAt = DateTime.UtcNow,
            MessageType = MessageType.System
        };

        // Сохраняем системное сообщение
        await _messageRepository.AddAsync(systemMessage);

        // Отправляем системное сообщение всем оставшимся участникам
        foreach (var p in remainingParticipants)
        {
            await Clients.User(p.UserId.ToString())
                .SendAsync(HubConstants.MessageEvents.MessageReceived, systemMessage);
        }

        // Уведомляем пользователя об успешном выходе
        await Clients.User(currentUserId.ToString())
            .SendAsync(HubConstants.ChatEvents.ChatDeleted, chatId);
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
}
