using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos;

public class UnidadStrategy : IEntidadStrategy
{
    private readonly ITransactionalRepository _transactionalRepository;
    private readonly IReadOnlyQuery _query;
    private readonly IApplicationDbContext _context;

    public UnidadStrategy(IReadOnlyQuery query, ITransactionalRepository transactionalRepository, IApplicationDbContext context)
    {
        _query = query;
        _transactionalRepository = transactionalRepository;
        _context = context;
    }



    public Task<bool> ActualizarRequerimientos(RequerimientoDto requerimiento)
    {
        throw new System.NotImplementedException();
    }
}
