using DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DAL.EntityFramework.GenericRepository;

public class GenericRepository<TEntity>(DbestateContext ctx) : IGenericRepository<TEntity> where TEntity : class
{
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await ctx.AddAsync(entity);
        await ctx.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
    {
        await ctx.AddRangeAsync(entity);
        await ctx.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        ctx.Remove(entity);
        await ctx.SaveChangesAsync();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await ctx.Set<TEntity>().IgnoreQueryFilters().FirstOrDefaultAsync(filter)
            : await ctx.Set<TEntity>().FirstOrDefaultAsync(filter);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null, bool ignoreQueryFilters = false)
    {
        return filter is null
            ? ignoreQueryFilters
                ? await ctx.Set<TEntity>().IgnoreQueryFilters().ToListAsync()
                : await ctx.Set<TEntity>().ToListAsync()
            : ignoreQueryFilters
                ? await ctx.Set<TEntity>().Where(filter).IgnoreQueryFilters().ToListAsync()
                : await ctx.Set<TEntity>().Where(filter).ToListAsync();
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? ctx.Set<TEntity>().IgnoreQueryFilters().Where(filter)
            : ctx.Set<TEntity>().Where(filter);
    }
    public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>>? filter = null, bool ignoreQueryFilters = false)
    {
        return filter is null
            ? ignoreQueryFilters
                ? ctx.Set<TEntity>().IgnoreQueryFilters()
                : ctx.Set<TEntity>()
            : ignoreQueryFilters
                ? ctx.Set<TEntity>().Where(filter).IgnoreQueryFilters()
                : ctx.Set<TEntity>().Where(filter);
    }

    public async Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await ctx.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
    }

    public async Task<TEntity?> GetAsNoTrackingWithIdentityResolutionAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await ctx.Set<TEntity>().AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(filter);
    }

    public IQueryable<TEntity> GetAsNoTrackingList(Expression<Func<TEntity, bool>>? filter = null)
    {
        return (filter is null
            ? ctx.Set<TEntity>().AsNoTracking()
            : ctx.Set<TEntity>().Where(filter)).AsNoTracking();
    }

    public IQueryable<TEntity> GetAsNoTrackingWithIdentityResolutionListAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return (filter is null
            ? ctx.Set<TEntity>().AsNoTrackingWithIdentityResolution()
            : ctx.Set<TEntity>().Where(filter)).AsNoTrackingWithIdentityResolution();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        ctx.Update(entity);
        await ctx.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entity)
    {
        ctx.UpdateRange(entity);
        await ctx.SaveChangesAsync();
        return entity;
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await ctx.Set<TEntity>().IgnoreQueryFilters().CountAsync(filter)
            : await ctx.Set<TEntity>().CountAsync(filter);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await ctx.Set<TEntity>().IgnoreQueryFilters().AnyAsync(filter)
            : await ctx.Set<TEntity>().AnyAsync(filter);
    }

    public async Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await ctx.Set<TEntity>().IgnoreQueryFilters().AllAsync(filter)
            : await ctx.Set<TEntity>().AllAsync(filter);
    }

    public async Task<TEntity?> GetAsync(Guid id)
    {
        return (await ctx.Set<TEntity>().FindAsync(id))!;
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
          ? await ctx.Set<TEntity>().IgnoreQueryFilters().SingleOrDefaultAsync(filter)
          : await ctx.Set<TEntity>().SingleOrDefaultAsync(filter);
    }

    public async Task<TEntity?> SingleAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
         ? await ctx.Set<TEntity>().IgnoreQueryFilters().SingleAsync(filter)
         : await ctx.Set<TEntity>().SingleAsync(filter);
    }

    public async Task<TEntity?> FirstAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false)
    {
        return ignoreQueryFilters
            ? await ctx.Set<TEntity>().IgnoreQueryFilters().FirstAsync(filter)
            : await ctx.Set<TEntity>().FirstAsync(filter);
    }

    public async Task DeleteRangeAsync(List<TEntity> entity)
    {
        ctx.RemoveRange(entity);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<TResult>> ExecuteRawSqlAsync<TResult>(string sql) where TResult : class, new()
    {
        using (var command = ctx.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            ctx.Database.OpenConnection();

            using (var result = await command.ExecuteReaderAsync())
            {
                var entities = new List<TResult>();

                while (await result.ReadAsync())
                {
                    var entity = new TResult();
                    for (var i = 0; i < result.FieldCount; i++)
                    {
                        var property = entity.GetType().GetProperty(result.GetName(i));
                        if (property != null && !DBNull.Value.Equals(result.GetValue(i)))
                        {
                            property.SetValue(entity, result.GetValue(i));
                        }
                    }

                    entities.Add(entity);
                }

                return entities;
            }
        }
    }
}