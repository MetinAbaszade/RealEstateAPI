using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Concrete;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<User> GetAsync(Expression<Func<User, bool>> filter)
    {
        return (await _dataContext.Users.Include(m => m.Role).FirstOrDefaultAsync(filter))!;
    }

    public IQueryable<User> GetList(Expression<Func<User, bool>>? filter = null)
    {
        return filter is null ?
              _dataContext.Users.Include(m => m.Role) :
              _dataContext.Users.Include(m => m.Role).Where(filter);
    }

    public async Task<IEnumerable<User>> GetListAsync(Expression<Func<User, bool>>? filter = null)
    {
        return filter is null ?
              await _dataContext.Users.Include(m => m.Role).ToListAsync() :
              await _dataContext.Users.Include(m => m.Role).Where(filter).ToListAsync();
    }

    public async Task<string?> GetUserSaltAsync(string email)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(m => m.Email == email);
        return user?.Salt;
    }

    public async Task<bool> IsUserExistAsync(string email, Guid? userId)
    {
        return await _dataContext.Users.AnyAsync(m => m.Email == email && m.Id != userId);
    }

    public Task UpdateUserAsync(User user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
        _dataContext.Entry(user).Property(m => m.Password).IsModified = false;
        _dataContext.Entry(user).Property(m => m.Salt).IsModified = false;
        _dataContext.Entry(user).Property(m => m.Image).IsModified = false;

        return Task.FromResult(1);
    }
}