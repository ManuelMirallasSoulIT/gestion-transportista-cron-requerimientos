using Domain.Entities.Presentacion;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace gestion_transportista_cron_requerimientos.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Task<bool> CreateTransaction(Func<IApplicationDbContext, Task> callback);
}
