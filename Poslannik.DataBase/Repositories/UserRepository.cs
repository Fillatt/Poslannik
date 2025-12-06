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

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var entities = await _context.Users.ToListAsync();
        return entities.Select(MapToModel);
    }

    public async Task<User?> GetUserByLoginAsync(string login) {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        if (userEntity != null) return MapToModel(userEntity);
        return null;
    }

    public async Task AddAsync(User model)
    {
        var entity = MapToEntity(model);
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User model)
    {
        var entity = await _context.Users.FindAsync(model.Id);
        if (entity != null)
        {
            MapToEntity(model, entity);
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _context.Users.FindAsync((Guid)(object)id);
        if (entity != null)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }
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

    private UserEntity MapToEntity(User model)
    {
        return new UserEntity
        {
            Id = model.Id,
            Login = model.Login,
            UserName = model.UserName,
            GroupUser = model.GroupUser,
            PasswordHash = model.PasswordHash,
            PasswordSalt = model.PasswordSalt,
            PublicKey = model.PublicKey
        };
    }

    private void MapToEntity(User model, UserEntity entity)
    {
        entity.Login = model.Login;
        entity.UserName = model.UserName;
        entity.GroupUser = model.GroupUser;
        entity.PasswordHash = model.PasswordHash;
        entity.PasswordSalt = model.PasswordSalt;
        entity.PublicKey = model.PublicKey;
    }

    private User MapToModel(UserEntity entity)
    {
        return new User
        {
            Id = entity.Id,
            Login = entity.Login,
            UserName = entity.UserName,
            GroupUser = entity.GroupUser,
            PasswordHash = entity.PasswordHash,
            PasswordSalt = entity.PasswordSalt,
            PublicKey = entity.PublicKey
        };
    }

}