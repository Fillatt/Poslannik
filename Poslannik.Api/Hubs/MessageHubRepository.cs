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
    public class MessageHubRepository(IMessageRepository messageRepository) : IMessageHubRepository
    {
        public Task AddAsync(Message model) => messageRepository.AddAsync(model);

        public Task DeleteAsync(long id) => messageRepository.DeleteAsync(id);

        public Task<IEnumerable<Message>> GetAllAsync() => messageRepository.GetAllAsync();

        public Task UpdateAsync(Message model) => messageRepository.UpdateAsync(model);
    }
}
