using System;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Task<bool> CreateTransaction(Func<IApplicationDbContext, Task> callback);
}
