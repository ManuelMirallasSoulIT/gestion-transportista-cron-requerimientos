using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.UseCase.V1.EventosOperation.Queries.GetAll;

public sealed class GetAllEventosQueryHandler : IRequestHandler<GetAllEventosQuery, IEnumerable<Eventos>>
{
    private readonly IServiceProvider _container;

    public GetAllEventosQueryHandler(IServiceProvider container)
    {
        _container = container;
    }

    public async Task<IEnumerable<Eventos>> Handle(GetAllEventosQuery request, CancellationToken cancellationToken)
    {
        using var scope = _container.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IEventosService>();
        return await service.GetAll(request.TipoEvento);
    }
}
