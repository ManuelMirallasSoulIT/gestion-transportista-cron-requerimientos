
using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using Domain.Entities.Presentaciones;
using Elastic.Apm.Api;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using gestion_transportista_cron_requerimientos.Application.Factories.Implementations;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Andreani.ARQ.Core.Clases.Enums;

namespace gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos
{
    public class ChoferStrategy : IEntidadStrategy
    {
        private readonly ITransactionalRepository _transactionalRepository;
        private readonly IReadOnlyQuery _query;
        private readonly IApplicationDbContext _context;
        private readonly ILogger<EntidadStrategyFactory> _logger;
        private readonly IEventosService _eventosService;

        public ChoferStrategy(IReadOnlyQuery query, ITransactionalRepository transactionalRepository, IApplicationDbContext context, ILogger<EntidadStrategyFactory> logger, IEventosService eventosService)
        {
            _query = query;
            _transactionalRepository = transactionalRepository;
            _context = context;    
            _logger = logger;
            _eventosService = eventosService;
        }

        public async Task<bool> ActualizarRequerimientos(RequerimientoDto requerimiento)
        {
            try
            {
                return await _context.CreateTransaction(async context =>
                {
                    
                    var requerimientosEliminar = await _query
                        .From<Requerimientos>()
                        .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NOT NULL AND (Requisito = @Requisito AND Chofer = @Chofer))", new { requerimiento.Requisito, requerimiento.Chofer }, TypeWhere.OR)
                        .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NULL AND (Requisito = @Requisito))", new { requerimiento.Requisito }, TypeWhere.OR)
                        .Where(r => $"(@Requisito IS NULL AND @Chofer IS NOT NULL AND (Chofer = @Chofer))", new { requerimiento.Chofer }, TypeWhere.OR)
                        .Execute();

                    _logger.LogInformation($"Se eliminaron {requerimientosEliminar.Count()} requerimientos.");

                    _transactionalRepository.DeleteRange(requerimientosEliminar);

                    var nuevosRequerimientos = await RequerimientosParaActualizar(requerimiento);
                    _transactionalRepository.InsertRange(nuevosRequerimientos);

                    _logger.LogInformation($"Se agregaron {nuevosRequerimientos.Count()} nuevos requerimientos.");

                    var evento = new Eventos
                    {
                        Tipo_Evento = "??????",
                        Evento = "??????",
                        FechaHoraPublicado = DateTime.Now
                    };

                    await _eventosService.Update(evento);

                    _logger.LogInformation("Proceso de actualización de requerimientos completado correctamente.");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar los requerimientos.");
                throw;
            }
        }

        private async Task<List<Requerimientos>> RequerimientosParaActualizar(RequerimientoDto requerimientoDto)
        {
            try 
            {  
                var requisitosActivos = await _query
                                            .From<Requisitos>()
                                            .Where(r => $"Activo = 1 AND Entidad = 'C'", where: TypeWhere.AND)
                                            .Where(r => $"((@Requisito IS NOT NULL AND Id = @Requisito) OR @Requisito IS NULL)", new { requerimientoDto.Requisito })
                                            .Execute();

                var choferesActivos = await _query
                                            .From<Choferes>()
                                            .Where(r => $"Activo = 1", where: TypeWhere.AND)
                                            .Where(r => $"((@Chofer IS NOT NULL AND Id = @Chofer) OR @Chofer IS NULL)", new { requerimientoDto.Chofer })
                                            .Execute();

                // Agarrar el listado de ids de choferes y requisitos
                // SELECT * 
                // FROM RequisitosPresentados
                // WHERE
                // (0 = @RequisitosCount OR RequisitosPresentados.Requisito IN @Requisitos) AND
                // (0 = @choferesActivosCount OR RequisitosPresentados.Chofer IN @choferesActivos)
                var presentaciones = await _query
                                           .From<RequisitosPresentados>()
                                           .Where(rp => $"(0 = @RequisitosCount OR RequisitosPresentados.Requisito IN @Requisitos)", new { }, TypeWhere.AND)
                                           .Where(rp => $" (0 = @choferesActivosCount OR RequisitosPresentados.Chofer IN @choferesActivos", new { })
                                           .Execute();


                var result = choferesActivos
                       .SelectMany(chofer => requisitosActivos, (chofer, requisito) => new { chofer, requisito })
                       .Select(item => new Requerimientos
                       {
                           Chofer = item.chofer.Id,
                           Requisito = item.requisito.Id,
                           FechaCreacion = DateTime.Now,
                           UsuarioCreacion = requerimientoDto.Usuario,
                           Estado = presentaciones
                               .Where(m => m.Requisito.Id == item.requisito.Id)
                               .Select(m => m.Status)
                               .DefaultIfEmpty("No presentado")
                               .First(status => status == "A" || status == "P" || status == "Rechazado"),
                           Presentacion = presentaciones
                               .Where(m => m.Requisito.Id == item.requisito.Id && (m.Status == "A" || m.Status == "P" || m.Status == "Rechazado"))
                               .OrderByDescending(m => m.Id)
                               .Select(m => m.Id)
                               .FirstOrDefault()
                       });

                    _logger.LogInformation("Generación 'RequerimientosParaActualizar' completada.");

                    return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar 'RequerimientosParaActualizar'");
                throw; 
            }
        }

    }
}
