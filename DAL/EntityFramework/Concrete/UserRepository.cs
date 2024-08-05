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
        return (await _dataContext.Users.FirstOrDefaultAsync(filter))!;
    }

    public IQueryable<User> GetList(Expression<Func<User, bool>>? filter = null)
    {
        return filter is null ?
              _dataContext.Users :
              _dataContext.Users.Where(filter);
    }

    public async Task<IEnumerable<User>> GetListAsync(Expression<Func<User, bool>>? filter = null)
    {
        return filter is null ?
              await _dataContext.Users.ToListAsync() :
              await _dataContext.Users.Where(filter).ToListAsync();
    }

    public async Task<string?> GetUserSaltAsync(string contactNumber)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(m => m.ContactNumber == contactNumber);
        return user?.Salt;
    }

    public async Task<bool> IsUserExistbyContactNumberAsync(string contactNumber, Guid? userId)
    {
        return await _dataContext.Users.AnyAsync(m => m.ContactNumber == contactNumber && m.Id != userId);
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