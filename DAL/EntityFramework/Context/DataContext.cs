using CORE.Abstract;
using ENTITIES.Entities;
using ENTITIES.Entities.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.EntityFramework.Context;

public class DataContext(DbContextOptions<DataContext> options,
                         IServiceProvider serviceProvider) : DbContext(options)
{
    public required DbSet<User> Users { get; set; }
    public required DbSet<Token> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.EnableSensitiveDataLogging();
    }

    /* migration commands
      dotnet ef --startup-project ../API migrations add initial --context DataContext
      dotnet ef --startup-project ../API database update --context DataContext
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Token>().HasQueryFilter(m => !m.IsDeleted);
    }
}