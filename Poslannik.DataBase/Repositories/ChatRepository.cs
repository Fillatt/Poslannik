using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repositories
{
    public class ChatRepository : BaseRepository<ChatEntity, Chat>, IChatRepository
    {
        
    }
}
