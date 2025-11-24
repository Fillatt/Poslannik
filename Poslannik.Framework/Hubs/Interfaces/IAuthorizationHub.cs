using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poslannik.Framework.Requests;

namespace Poslannik.Framework.Hubs.Interfaces
{
    public interface IAuthorizationHub
    {
        /// <summary>
        /// Верификация пароля
        /// </summary>
        /// <param name="request">Данные запроса для верификации</param>
        public Task<bool> VerifyAsync(VerifyRequest request);
    }
}
