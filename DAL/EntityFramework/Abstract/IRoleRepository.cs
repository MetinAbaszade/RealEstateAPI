using CORE.Enums;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Abstract;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task ClearRolePermissionsAync(Guid id);
    IQueryable<Role> GetList(Expression<Func<Role, bool>>? filter = null);
    Task<IEnumerable<Role>> GetListAsync(Expression<Func<Role, bool>>? filter = null);
    Task<Role> GetAsync(Expression<Func<Role, bool>> filter);
    Task<Role> GetAsync(EUserType userType);
}