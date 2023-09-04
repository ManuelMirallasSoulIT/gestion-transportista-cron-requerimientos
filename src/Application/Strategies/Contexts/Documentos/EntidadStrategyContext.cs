using Application.Strategies;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Factories.Interfaces;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using System;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.Strategies.Contexts.Documentos;

public class EntidadStrategyContext 
{
    private readonly IEntidadStrategy _entidadStrategy;
    public EntidadStrategyContext(string entidad, IEntidadStrategyFactory strategyFactory)
    {
        _entidadStrategy = GetStrategy(entidad, strategyFactory);
    }

    private static IEntidadStrategy GetStrategy(string entidad, IEntidadStrategyFactory strategyFactory)
    {
        return entidad switch
        {
            "Chofer" => strategyFactory.CreateChoferStrategy(),
            "Proveedor" => strategyFactory.CreateProveedorStrategy(),
            "Unidad" => strategyFactory.CreateUnidadStrategy(),
            _ => throw new ArgumentException($"La entidad {entidad} no tiene una implementación de estrategia.")
        };
    }

    public async Task<bool> ExecuteAsync(RequerimientoDto requerimientoDto, Eventos evento)
        => await _entidadStrategy.ActualizarRequerimientos(requerimientoDto, evento);
}