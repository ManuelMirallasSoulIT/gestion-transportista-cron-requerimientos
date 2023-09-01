using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using System.Threading.Tasks;

namespace Application.Strategies;

public interface IEntidadStrategy
{
    Task<bool> ActualizarRequerimientos(RequerimientoDto requerimiento, Eventos evento);
}
