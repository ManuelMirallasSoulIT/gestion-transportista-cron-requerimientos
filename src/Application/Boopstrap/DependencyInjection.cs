using Andreani.ARQ.Core.Pipeline.Extension;
using Application.Strategies;
using FluentValidation;
using gestion_transportista_cron_requerimientos.Application.Factories.Implementations;
using gestion_transportista_cron_requerimientos.Application.Factories.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace gestion_transportista_cron_requerimientos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAndreaniPipeline(Verbose: true);
        services.AddScoped<IEntidadStrategy, ChoferStrategy>();
        services.AddScoped<IEntidadStrategy, UnidadStrategy>();
        services.AddScoped<IEntidadStrategy, ProveedorStrategy>();
        services.AddScoped<IEntidadStrategy, RequisitoStrategy>();
        services.AddScoped<IEntidadStrategyFactory, EntidadStrategyFactory>();
        services.AddOptions();

        return services;
    }
}
