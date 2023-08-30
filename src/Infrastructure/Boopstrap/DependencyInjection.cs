using Andreani.ARQ.CQRS.SqlServer.Extension;
using Andreani.Data.CQRS.Extension;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using gestion_transportista_cron_requerimientos.Infrastructure.Persistence;
using gestion_transportista_cron_requerimientos.Infrastructure.Services.Evento;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace gestion_transportista_cron_requerimientos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCQRS<ApplicationDbContext>(configuration).UseSqlServer();
        
        services.AddScoped<ApplicationDbContext>();

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>()
        );

        services.AddTransient<IEventosService, EventosService>();

        return services;
    }
}
