using Poslannik.DataBase.Entities;
using Poslannik.Framework.Models;

namespace Poslannik.DataBase.Repositories.Interfaces;

public interface IMessageRepository : IRepository<MessageEntity, Message>
{
    Task<IReadOnlyList<Message>> GetAllMessageByChatId(Guid chatId);
}
