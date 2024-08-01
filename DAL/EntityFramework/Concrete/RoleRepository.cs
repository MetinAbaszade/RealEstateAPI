using CORE.Enums;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;
using DAL.EntityFramework.GenericRepository;
using ENTITIES.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.EntityFramework.Concrete;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private readonly DataContext _dataContext;

    public RoleRepository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task ClearRolePermissionsAync(Guid id)
    {
        var role = await _dataContext.Roles.Include(m => m.Permissions).FirstOrDefaultAsync(m => m.Id == id);
        role!.Permissions!.Clear();
        _dataContext.Entry(role).State = EntityState.Detached;
        await _dataContext.SaveChangesAsync();
    }

    public async Task<Role> GetAsync(Expression<Func<Role, bool>> filter)
    {
        return (await _dataContext.Roles.Include(m => m.Permissions).FirstOrDefaultAsync(filter!))!;
    }

    public async Task<Role> GetAsync(EUserType userType)
    {
        return (await _dataContext.Roles.FirstOrDefaultAsync(m => m.Key == userType.ToString()))!;
    }

    public IQueryable<Role> GetList(Expression<Func<Role, bool>>? filter)
    {
        return filter is null ?
              _dataContext.Roles.Include(m => m.Permissions) :
              _dataContext.Roles.Where(filter)
                                .Include(m => m.Permissions);
    }

    public async Task<IEnumerable<Role>> GetListAsync(Expression<Func<Role, bool>>? filter)
    {
        return filter is null ?
              await _dataContext.Roles.Include(m => m.Permissions).ToListAsync() :
              await _dataContext.Roles.Where(filter)
                                      .Include(m => m.Permissions)
                                      .ToListAsync();
    }
}