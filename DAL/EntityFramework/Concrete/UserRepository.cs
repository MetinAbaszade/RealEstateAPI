using DAL.Context;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Concrete;

public class UserRepository(DbestateContext dataContext) : GenericRepository<User>(dataContext), IUserRepository
{
    public IQueryable<User> GetList(Expression<Func<User, bool>>? filter = null)
    {
        return filter is null ?
              dataContext.Users :
              dataContext.Users.Where(filter);
    }

    public async Task<IEnumerable<User>> GetListAsync(Expression<Func<User, bool>>? filter = null)
    {
        return filter is null ?
              await dataContext.Users.ToListAsync() :
              await dataContext.Users.Where(filter).ToListAsync();
    }

    public async Task<string?> GetUserSaltAsync(string contactNumber)
    {
        var user = await dataContext.Users.SingleOrDefaultAsync(m => m.ContactNumber == contactNumber);
        return user?.Salt;
    }

    public async Task<bool> IsUserExistbyContactNumberAsync(string contactNumber)
    {
        return await dataContext.Users.AnyAsync(m => m.ContactNumber == contactNumber);
    }

    public Task UpdateUserAsync(User user)
    {
        dataContext.Entry(user).State = EntityState.Modified;
        dataContext.Entry(user).Property(m => m.Password).IsModified = false;
        dataContext.Entry(user).Property(m => m.Salt).IsModified = false;

        return Task.FromResult(1);
    }
}