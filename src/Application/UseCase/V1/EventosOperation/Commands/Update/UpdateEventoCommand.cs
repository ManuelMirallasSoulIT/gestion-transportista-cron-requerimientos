using GDT.Common.Domain.Entities;
using MediatR;

namespace gestion_transportista_cron_requerimientos.Application.UseCase.V1.EventosOperation.Commands.Update;

public sealed class UpdateEventoCommand : IRequest<bool>
{
    public Eventos Evento { get; set; }
}
