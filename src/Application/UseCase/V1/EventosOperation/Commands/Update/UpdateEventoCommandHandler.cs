using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.UseCase.V1.EventosOperation.Commands.Update;

public class UpdateEventoCommandHandler : IRequestHandler<UpdateEventoCommand, bool>
{
    private readonly IServiceProvider _container;

    public UpdateEventoCommandHandler(IServiceProvider container)
    {
        _container = container;
    }

    public async Task<bool> Handle(UpdateEventoCommand request, CancellationToken cancellationToken)
    {
        using var scope = _container.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<IEventosService>();
        
        var evento = request.Evento;

        evento.FechaHoraPublicado = DateTime.Now;

        await service.Update(evento);

        return true;
    }
}
