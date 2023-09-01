using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using gestion_transportista_cron_requerimientos.Application.Factories.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos;
using Microsoft.Extensions.Logging;


namespace gestion_transportista_cron_requerimientos.Application.Factories.Implementations;

public class EntidadStrategyFactory : IEntidadStrategyFactory
{
    private readonly ITransactionalRepository _transactionalRepository;
    private readonly IReadOnlyQuery _query;
    private readonly IApplicationDbContext _context;
    private readonly ILogger<EntidadStrategyFactory> _logger;
    private readonly IEventosService _eventosService;

    public EntidadStrategyFactory(ITransactionalRepository transactionalRepository, IReadOnlyQuery query, IApplicationDbContext context, ILogger<EntidadStrategyFactory> logger, IEventosService eventosService)
    {
        _transactionalRepository = transactionalRepository;
        _query = query;
        _context = context;  
        _logger = logger;
        _eventosService = eventosService;
    }

    public IEntidadStrategy CreateRequisitoStrategy() =>
        new RequisitoStrategy(_query, _transactionalRepository, _context);

    public IEntidadStrategy CreateChoferStrategy() =>
         new ChoferStrategy(_query, _transactionalRepository, _context, _logger, _eventosService);

    public IEntidadStrategy CreateProveedorStrategy() =>
        new ProveedorStrategy(_query, _transactionalRepository, _context);

    public IEntidadStrategy CreateUnidadStrategy() =>
        new UnidadStrategy(_query, _transactionalRepository, _context);
}
