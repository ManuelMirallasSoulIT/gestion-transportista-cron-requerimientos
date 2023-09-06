using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using gestion_transportista_cron_requerimientos.Application.Factories.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos;
using Serilog;

namespace gestion_transportista_cron_requerimientos.Application.Factories.Implementations;

public class EntidadStrategyFactory : IEntidadStrategyFactory
{
    private readonly ITransactionalRepository _transactionalRepository;
    private readonly IReadOnlyQuery _query;
    private readonly ILogger _logger;
    private readonly IEventosService _eventosService;

    public EntidadStrategyFactory(
        ITransactionalRepository transactionalRepository, 
        IReadOnlyQuery query,
        ILogger logger,
        IEventosService eventosService)
    {
        _transactionalRepository = transactionalRepository;
        _query = query;
        _logger = logger;
        _eventosService = eventosService;
    }

    public IEntidadStrategy CreateChoferStrategy() =>
         new ChoferStrategy(_query, _transactionalRepository, _logger, _eventosService);

    public IEntidadStrategy CreateProveedorStrategy() =>
        new ProveedorStrategy(_query, _transactionalRepository, _logger, _eventosService);

    public IEntidadStrategy CreateUnidadStrategy() =>
        new UnidadStrategy(_query, _transactionalRepository, _logger, _eventosService);
}
