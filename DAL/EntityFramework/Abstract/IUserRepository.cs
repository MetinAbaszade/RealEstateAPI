using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsUserExistbyContactNumberAsync(string contactNumber);
    Task<string?> GetUserSaltAsync(string contactNumber);
    Task UpdateUserAsync(User user);
    IQueryable<User> GetList(Expression<Func<User, bool>>? filter = null);
    Task<IEnumerable<User>> GetListAsync(Expression<Func<User, bool>>? filter = null);
}