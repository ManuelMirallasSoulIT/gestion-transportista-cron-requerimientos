using GDT.Common.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace gestion_transportista_cron_requerimientos.Application.UseCase.V1.EventosOperation.Queries.GetAll;

public sealed class GetAllEventosQuery : IRequest<IEnumerable<Eventos>>
{
    public string TipoEvento { get; set; }
}
