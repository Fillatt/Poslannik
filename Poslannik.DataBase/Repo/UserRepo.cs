using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poslannik.DataBase.Repo
{
    public class UserRepo
    {
        private readonly ApplicationContext _dbContext;

        public UserRepo(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<User?> GetUserById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public Task<Guid?> GetUserIdByLogin(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .Where(u => u.Login == login)
                .Select(u => (Guid?)u.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
        }

        public Task<List<User>> GetUsersByIds(List<Guid> userIds, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<User> CreateUser(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task UpdateUser(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task<bool> UserExists(string login, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .AnyAsync(u => u.Login == login, cancellationToken);
        }
    }
}
