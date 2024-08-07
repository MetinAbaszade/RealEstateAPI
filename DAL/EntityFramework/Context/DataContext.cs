using CORE.Abstract;
using ENTITIES.Entities;
using ENTITIES.Entities.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.EntityFramework.Context;

public class DataContext(DbContextOptions<DataContext> options, IServiceProvider serviceProvider) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Token> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.EnableSensitiveDataLogging();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditProperties();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /* migration commands
      dotnet ef --startup-project ../API migrations add initial --context DataContext
      dotnet ef --startup-project ../API database update --context DataContext
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddGlobalFilter("IsDeleted", false);

        modelBuilder.Entity<Token>().HasQueryFilter(m => !m.IsDeleted);
    }

    private void SetAuditProperties()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Auditable && e.State is EntityState.Added or EntityState.Modified);

        var tokenService = serviceProvider.GetRequiredService<ITokenService>();

        foreach (var entityEntry in entries)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    ((Auditable)entityEntry.Entity).CreatedAt = DateTime.Now;
                    ((Auditable)entityEntry.Entity).CreatedById =
                        tokenService.GetUserIdFromToken();
                    break;
                case EntityState.Modified:
                    {
                        Entry((Auditable)entityEntry.Entity).Property(p => p.CreatedAt).IsModified =
                            false;
                        Entry((Auditable)entityEntry.Entity).Property(p => p.CreatedById).IsModified =
                            false;

                        if (((Auditable)entityEntry.Entity).IsDeleted)
                        {
                            Entry((Auditable)entityEntry.Entity).Property(p => p.ModifiedBy)
                                .IsModified = false;
                            Entry((Auditable)entityEntry.Entity).Property(p => p.ModifiedAt)
                                .IsModified = false;

                            ((Auditable)entityEntry.Entity).DeletedAt = DateTime.Now;
                            ((Auditable)entityEntry.Entity).DeletedBy =
                                tokenService.GetUserIdFromToken();
                        }
                        else
                        {
                            ((Auditable)entityEntry.Entity).ModifiedAt = DateTime.Now;
                            ((Auditable)entityEntry.Entity).ModifiedBy =
                                tokenService.GetUserIdFromToken();
                        }

                        break;
                    }
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}