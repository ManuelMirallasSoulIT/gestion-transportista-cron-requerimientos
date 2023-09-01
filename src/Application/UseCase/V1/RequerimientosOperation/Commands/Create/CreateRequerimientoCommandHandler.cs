using gestion_transportista_cron_requerimientos.Application.Factories.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Strategies.Contexts.Documentos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.UseCase.V1.RequerimientosOperation.Commands.Create;

public class CreateRequerimientoCommandHandler : IRequestHandler<CreateRequerimientoCommand, bool>
{
    private readonly IServiceProvider _container;

    public CreateRequerimientoCommandHandler(IServiceProvider container)
    {
        _container = container;
    }
    public async Task<bool> Handle(CreateRequerimientoCommand request, CancellationToken cancellationToken)
    {
        using var scope = _container.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<IEntidadStrategyFactory>();
        var strategy = new EntidadStrategyContext(request.RequerimientoDto.Tipo, factory);
        return await strategy.ExecuteAsync(request.RequerimientoDto, request.Evento);
    }
}
