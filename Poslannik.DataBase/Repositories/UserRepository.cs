using Microsoft.EntityFrameworkCore;
using Poslannik.DataBase.Entities;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Models;
using Poslannik.Framework.Services;

namespace Poslannik.DataBase.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task AddTestUserAsync()
    {
        var password = "123";
        var login = "maxim";

        byte[] passwordHash;
        byte[] passwordSalt;
        PasswordHasher.CreatePasswordHash(password, out passwordHash, out passwordSalt);

        var user = new UserEntity
        {
            Login = login,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<PasswordDataType, byte[]>?> GetPasswordDataByLoginAsync(string login)
    {
        var entity = await _context.Users.Where(x => x.Login == login).FirstOrDefaultAsync();
        if (entity != null) return new Dictionary<PasswordDataType, byte[]>()
        {
            { PasswordDataType.Hash, entity.PasswordHash},
            { PasswordDataType.Salt, entity.PasswordSalt} 
        };
        else return null;
    }

    public Task<bool> HasUserByLoginAsync(string login) => _context.Users.Where(x => x.Login == login).AnyAsync();
    

    private UserEntity MapToEntity(User user)
    {
        byte[] passwordHash;
        byte[] passwordSalt;
        PasswordHasher.CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

        return new UserEntity
        {
            Id = user.Id,
            Login = user.Login,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserName = user.UserName,
            GroupUser = user.GroupUser,
            PublicKey = user.PublicKey
        };
    }
}