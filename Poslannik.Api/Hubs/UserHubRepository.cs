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
    public class UserHubRepository(IUserRepository userRepository) : IUserHubRepository
    {
       
    }
}
