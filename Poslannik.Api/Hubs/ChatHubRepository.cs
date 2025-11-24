using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Hubs.Interfaces;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;

namespace Poslannik.Framework.Hubs
{
    public class ChatHubRepository (IChatRepository chatRepository) : IChatHubRepository
    {
        private readonly IChatRepository _chatRepository = chatRepository;

        public Task AddAsync(Chat model) =>  _chatRepository.AddAsync(model);

        public Task DeleteAsync(long id) => _chatRepository.DeleteAsync(id);

        public Task<IEnumerable<Chat>> GetAllAsync() => _chatRepository.GetAllAsync();

        public Task UpdateAsync(Chat model) => UpdateAsync(model);
    }
}
