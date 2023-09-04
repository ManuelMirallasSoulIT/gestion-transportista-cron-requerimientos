using Application.Strategies;

namespace gestion_transportista_cron_requerimientos.Application.Factories.Interfaces;

public interface IEntidadStrategyFactory
{
    IEntidadStrategy CreateChoferStrategy();
    IEntidadStrategy CreateProveedorStrategy();
    IEntidadStrategy CreateUnidadStrategy();
}
