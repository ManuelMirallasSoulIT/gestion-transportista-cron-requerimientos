using gestion_transportista_cron_requerimientos.Domain.Common;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
