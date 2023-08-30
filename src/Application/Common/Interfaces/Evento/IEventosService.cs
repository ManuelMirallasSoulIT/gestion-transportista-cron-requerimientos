using GDT.Common.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;

public interface IEventosService
{
    Task<IEnumerable<Eventos>> GetAll(string tipoEvento);
    Task Update(Eventos evento);
}
