using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Eventos> Eventos { get; set; }
    public DbSet<Requerimientos> Requerimientos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CreateTransaction(Func<IApplicationDbContext, Task> callback)
    {
        using var transaction = await base.Database.BeginTransactionAsync();

        try
        {
            await callback.Invoke(this);
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, $"For application {applicationType}. Occurred the following error: {ex.Message}");
            await transaction.RollbackAsync();
            return false;
        }

    }
}
