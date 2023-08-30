using gestion_transportista_cron_requerimientos.Application.UseCase.V1.EventosOperation.Commands.Update;
using gestion_transportista_cron_requerimientos.Application.UseCase.V1.EventosOperation.Queries.GetAll;
using gestion_transportista_cron_requerimientos.Application.UseCase.V1.RequerimientosOperation.Commands.Create;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Services;

public class RequerimientosServices : BackgroundService
{
    private readonly ILogger<RequerimientosServices> _logger;
    private readonly ISender _mediator;
    private readonly int _miliseconds;

    public RequerimientosServices(ILogger<RequerimientosServices> logger, IConfiguration configuration, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
        _miliseconds = configuration.GetValue<int>("Publisher:Delay");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var eventos = await _mediator.Send(new GetAllEventosQuery() { TipoEvento = "ActualizarRequerimiento" }, stoppingToken);

                if (eventos.Any())
                {
                    foreach (var evento in eventos)
                    {
                        var requerimiento = JsonConvert.DeserializeObject<RequerimientoDto>(evento.Evento);
                        var saved = await _mediator.Send(new CreateRequerimientoCommand() { RequerimientoDto = requerimiento }, stoppingToken);

                        if (!saved) continue;
                        
                        await _mediator.Send(new UpdateEventoCommand() { Evento = evento }, stoppingToken);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
            }
            await Task.Delay(_miliseconds, stoppingToken);
        }
    }
}
