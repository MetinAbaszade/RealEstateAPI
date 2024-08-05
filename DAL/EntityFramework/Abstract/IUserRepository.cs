using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsUserExistbyContactNumberAsync(string contactNumber, Guid? userId);
    Task<string?> GetUserSaltAsync(string contactNumber);
    Task UpdateUserAsync(User user);
    Task<User> GetAsync(Expression<Func<User, bool>> filter);
    IQueryable<User> GetList(Expression<Func<User, bool>>? filter = null);
    Task<IEnumerable<User>> GetListAsync(Expression<Func<User, bool>>? filter = null);
}