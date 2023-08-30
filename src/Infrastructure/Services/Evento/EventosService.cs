using Andreani.ARQ.Core.Interface;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Andreani.ARQ.Core.Clases.Enums;

namespace gestion_transportista_cron_requerimientos.Infrastructure.Services.Evento;

public class EventosService : IEventosService
{
    private readonly ITransactionalRepository _repository;
    private readonly IReadOnlyQuery _query;

    public EventosService(ITransactionalRepository repository, IReadOnlyQuery query)
    {
        _repository = repository;
        _query = query;
    }

    public async Task<IEnumerable<Eventos>> GetAll(string tipoEvento) =>
        await _query
                .From<Eventos>()
                .Where(m => $"{nameof(m.Tipo_Evento)} = @TipoEvento", new { TipoEvento = tipoEvento}, TypeWhere.AND)
                .Where(m => $"{nameof(m.FechaHoraPublicado)} IS NULL")
                .Execute();

    public async Task Update(Eventos evento)
    {
        _repository.Update(evento);
        await _repository.SaveChangeAsync();
    }
}
