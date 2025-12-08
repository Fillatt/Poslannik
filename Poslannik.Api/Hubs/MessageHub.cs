using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Hubs;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Models;
using System.Security.Claims;
using System.Text;

namespace Poslannik.Api.Hubs;

[Authorize]
public class MessageHub : Hub, IMessageHubRepository
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChatRepository _chatRepository;

    public MessageHub(
        IMessageRepository messageRepository,
        IChatRepository chatRepository)
    {
        _messageRepository = messageRepository;
        _chatRepository = chatRepository;
    }

    private Guid GetCurrentUserId()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User not authenticated");

        return Guid.Parse(userId);
    }

    public async Task<Message?> SendMessageAsync(Guid chatId, string messageText)
    {
        var senderId = GetCurrentUserId();

        // Получаем чат для определения получателей
        var chat = await _chatRepository.GetByIdAsync(chatId);
        if (chat == null)
            return null;

        // Проверяем доступ пользователя к чату
        bool hasAccess = false;
        List<Guid> recipientIds = new List<Guid>();

        if (chat.ChatType == ChatType.Private)
        {
            // Для личного чата проверяем, что пользователь - один из участников
            if (chat.User1Id == senderId)
            {
                hasAccess = true;
                if (chat.User2Id.HasValue)
                    recipientIds.Add(chat.User2Id.Value);
            }
            else if (chat.User2Id == senderId)
            {
                hasAccess = true;
                if (chat.User1Id.HasValue)
                    recipientIds.Add(chat.User1Id.Value);
            }
        }
        else if (chat.ChatType == ChatType.Group)
        {
            // Для группового чата проверяем участников через Participants
            // Здесь нужно получить участников из базы данных
            // Пока упрощаем - считаем что если чат существует, доступ есть
            hasAccess = true;
            // TODO: Получить список всех участников группового чата кроме отправителя
            // recipientIds = await GetGroupChatParticipantsAsync(chatId, senderId);
        }

        if (!hasAccess)
            return null;

        // Конвертируем текст сообщения в байты (простое хранение без шифрования)
        var messageBytes = Encoding.UTF8.GetBytes(messageText);

        // Создаем сообщение
        var message = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            SenderId = senderId,
            EncryptedMessage = messageBytes,
            SentAt = DateTime.UtcNow,
            MessageType = MessageType.Text
        };

        // Сохраняем сообщение в БД
        await _messageRepository.AddAsync(message);

        // Отправляем сообщение получателям
        foreach (var recipientId in recipientIds)
        {
            await Clients.User(recipientId.ToString())
                .SendAsync(HubConstants.MessageEvents.MessageReceived, message);
        }

        // Для групповых чатов отправляем всем участникам
        if (chat.ChatType == ChatType.Group)
        {
            await Clients.Group(chatId.ToString())
                .SendAsync(HubConstants.MessageEvents.MessageReceived, message);
        }

        // Подтверждаем отправку отправителю
        await Clients.User(senderId.ToString())
            .SendAsync(HubConstants.MessageEvents.MessageSent, message);

        return message;
    }

    public async Task<IEnumerable<Message>> GetChatMessagesAsync(Guid chatId)
    {
        var userId = GetCurrentUserId();

        // Проверяем, что пользователь имеет доступ к чату
        var chat = await _chatRepository.GetByIdAsync(chatId);
        if (chat == null)
            return Enumerable.Empty<Message>();

        // Для личного чата проверяем, что пользователь - участник
        if (chat.ChatType == ChatType.Private)
        {
            if (chat.User1Id != userId && chat.User2Id != userId)
                return Enumerable.Empty<Message>();
        }
        // TODO: Для группового чата проверить участие через Participants

        // Получаем все сообщения чата (уже отсортированы по времени)
        return await _messageRepository.GetMessagesByChatIdAsync(chatId);
    }

    #region IMessageHubRepository legacy methods
    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _messageRepository.GetAllAsync();
    }

    public async Task AddAsync(Message model)
    {
        await _messageRepository.AddAsync(model);
    }

    public async Task UpdateAsync(Message model)
    {
        await _messageRepository.UpdateAsync(model);
    }

    public async Task DeleteAsync(long id)
    {
        await _messageRepository.DeleteAsync(id);
    }
    #endregion
}
