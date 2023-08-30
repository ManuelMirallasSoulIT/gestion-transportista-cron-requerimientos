using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Factories.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos;

namespace gestion_transportista_cron_requerimientos.Application.Factories.Implementations;

public class EntidadStrategyFactory : IEntidadStrategyFactory
{
    private readonly ITransactionalRepository _transactionalRepository;
    private readonly IReadOnlyQuery _query;
    private readonly IApplicationDbContext _context;

    public EntidadStrategyFactory(ITransactionalRepository transactionalRepository, IReadOnlyQuery query, IApplicationDbContext context)
    {
        _transactionalRepository = transactionalRepository;
        _query = query;
        _context = context;
    }

    public IEntidadStrategy CreateRequisitoStrategy() =>
        new RequisitoStrategy(_query, _transactionalRepository, _context);

    public IEntidadStrategy CreateChoferStrategy() =>
         new ChoferStrategy(_query, _transactionalRepository, _context);

    public IEntidadStrategy CreateProveedorStrategy() =>
        new ProveedorStrategy(_query, _transactionalRepository, _context);

    public IEntidadStrategy CreateUnidadStrategy() =>
        new UnidadStrategy(_query, _transactionalRepository, _context);
}
