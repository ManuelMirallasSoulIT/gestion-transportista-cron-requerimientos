using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos;

public class UnidadStrategy : IEntidadStrategy
{
    private readonly ITransactionalRepository _transactionalRepository;
    private readonly IReadOnlyQuery _query;

    public UnidadStrategy(
        IReadOnlyQuery query, 
        ITransactionalRepository transactionalRepository)
    {
        _query = query;
        _transactionalRepository = transactionalRepository;
    }

    public Task<bool> ActualizarRequerimientos(RequerimientoDto requerimiento, Eventos evento)
    {
        throw new System.NotImplementedException();
    }
}
