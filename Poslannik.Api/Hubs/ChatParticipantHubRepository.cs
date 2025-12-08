using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs;

public class ChatParticipantHubRepository (IChatParticipantRepository chatParticipantRepository) : IChatParticipantHubRepository
{
    public Task AddAsync(ChatParticipant model) => chatParticipantRepository.AddAsync(model);

    public Task DeleteAsync(long id) => chatParticipantRepository.DeleteAsync(id);

    public Task<IEnumerable<ChatParticipant>> GetAllAsync()  => chatParticipantRepository.GetAllAsync();

    public Task UpdateAsync(ChatParticipant model) => chatParticipantRepository.UpdateAsync(model);
}
