using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using MediatR;

namespace gestion_transportista_cron_requerimientos.Application.UseCase.V1.RequerimientosOperation.Commands.Create;

public sealed class CreateRequerimientoCommand : IRequest<bool>
{
    public RequerimientoDto RequerimientoDto { get; set; }
}
