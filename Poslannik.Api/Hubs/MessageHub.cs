using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs;

[Authorize]
public class MessageHub(
    IMessageRepository messageRepository, 
    IChatParticipantRepository chatParticipantRepository,
    IChatRepository chatRepository) : Hub, IMessageHub
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IChatParticipantRepository _chatParticipantRepository = chatParticipantRepository;
    private readonly IChatRepository _chatRepository = chatRepository;

    public async Task SendMessageAsync(Message message)
    {
        _messageRepository?.AddAsync(message);
        var chat = await _chatRepository.GetChatByIdAsync(message.ChatId);

        if (chat.ChatType == ChatType.Private)
        {
            await Clients.User(chat.User1Id!.Value.ToString())
                 .SendAsync(HubConstants.MessageEvents.MessageSended, message);
            await Clients.User(chat.User2Id!.Value.ToString())
                .SendAsync(HubConstants.MessageEvents.MessageSended, message);
        }
        else
        {
            var participants = await _chatParticipantRepository.GetAllAsync();
            var chatParticipants = participants.Where(p => p.ChatId == chat.Id).ToList();

            foreach (var participant in chatParticipants)
            {
                await Clients.User(participant.UserId.ToString())
                    .SendAsync(HubConstants.MessageEvents.MessageSended, message);
            }
        }
    }
}
