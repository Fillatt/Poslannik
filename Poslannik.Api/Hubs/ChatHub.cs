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

    public ChatHub(
        IChatRepository chatRepository,
        IChatParticipantRepository chatParticipantRepository)
    {
        _chatRepository = chatRepository;
        _chatParticipantRepository = chatParticipantRepository;
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
    public async Task<Chat> CreateChatAsync(Chat chat)
    {
        // Создаем новый чат с уникальным идентификатором
        var newChat = chat with { Id = Guid.NewGuid() };

        // Сохраняем чат в базе данных
        await _chatRepository.AddAsync(newChat);

        // Уведомляем всех участников о создании чата
        await NotifyChatParticipantsAsync(newChat.Id, newChat);

        return newChat;
    }

    /// <summary>
    /// Уведомляет всех участников чата о событии
    /// </summary>
    public async Task NotifyChatParticipantsAsync(Guid chatId, Chat chat)
    {
        // Получаем всех участников чата
        var participants = await _chatParticipantRepository.GetAllAsync();
        var chatParticipants = participants.Where(p => p.ChatId == chatId).ToList();

        // Отправляем уведомление каждому участнику
        foreach (var participant in chatParticipants)
        {
            // Отправляем уведомление конкретному пользователю
            await Clients.User(participant.UserId.ToString())
                .SendAsync(HubConstants.ChatEvents.ChatCreated, chat);
        }
    }

    /// <summary>
    /// Обновляет информацию о чате и уведомляет участников
    /// </summary>
    public async Task UpdateChatAsync(Chat chat)
    {
        await _chatRepository.UpdateAsync(chat);

        // Уведомляем участников об обновлении
        await NotifyUpdateAsync(chat.Id, chat);
    }

    /// <summary>
    /// Удаляет чат и уведомляет участников
    /// </summary>
    public async Task DeleteChatAsync(Guid chatId)
    {
        await _chatRepository.DeleteAsync((long)(object)chatId);

        // Уведомляем участников об удалении
        await NotifyDeleteAsync(chatId);
    }

    /// <summary>
    /// Уведомляет участников об обновлении чата
    /// </summary>
    private async Task NotifyUpdateAsync(Guid chatId, Chat chat)
    {
        var participants = await _chatParticipantRepository.GetAllAsync();
        var chatParticipants = participants.Where(p => p.ChatId == chatId).ToList();

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
