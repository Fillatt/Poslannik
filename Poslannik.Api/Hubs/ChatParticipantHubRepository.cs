using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs;

public class ChatParticipantHubRepository (IChatParticipantRepository chatParticipantRepository) : IChatParticipantHubRepository
{
    public Task AddAsync(ChatParticipant model) => chatParticipantRepository.AddAsync(model);

    public Task DeleteAsync(Guid id) => chatParticipantRepository.DeleteAsync(id);

    public Task<IEnumerable<ChatParticipant>> GetAllAsync()  => chatParticipantRepository.GetAllAsync();

    public Task UpdateAsync(ChatParticipant model) => chatParticipantRepository.UpdateAsync(model);

    public Task<IEnumerable<ChatParticipant>> GetByChatIdAsync(Guid chatId) => chatParticipantRepository.GetByChatIdAsync(chatId);

    public Task DeleteByUserAndChatAsync(Guid chatId, Guid userId) => chatParticipantRepository.DeleteByUserAndChatAsync(chatId, userId);
}
