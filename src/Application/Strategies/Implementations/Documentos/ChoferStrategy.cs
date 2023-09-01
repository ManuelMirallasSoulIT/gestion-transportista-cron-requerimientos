
using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using Domain.Entities.Presentacion;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using gestion_transportista_cron_requerimientos.Application.Factories.Implementations;
using gestion_transportista_cron_requerimientos.Application.Helpers;
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
        private readonly ITransactionalRepository _repository;
        private readonly IReadOnlyQuery _query;
        private readonly ILogger<EntidadStrategyFactory> _logger;
        private readonly IEventosService _eventosService;

        public ChoferStrategy(
            IReadOnlyQuery query, 
            ITransactionalRepository transactionalRepository,
            ILogger<EntidadStrategyFactory> logger, 
            IEventosService eventosService)
        {
            _query = query;
            _repository = transactionalRepository;
            _logger = logger;
            _eventosService = eventosService;
        }

        public async Task<bool> ActualizarRequerimientos(RequerimientoDto requerimiento, Eventos evento)
        {
            try
            {
                var requerimientosEliminar = await _query
                                                    .From<Requerimientos>()
                                                    .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NOT NULL AND (Requisito = @Requisito AND Chofer = @Chofer))", new { requerimiento.Requisito, requerimiento.Chofer }, TypeWhere.OR)
                                                    .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NULL AND (Requisito = @Requisito))", new { requerimiento.Requisito }, TypeWhere.OR)
                                                    .Where(r => $"(@Requisito IS NULL AND @Chofer IS NOT NULL AND (Chofer = @Chofer))", new { requerimiento.Chofer }, TypeWhere.OR)
                                                    .Execute();


                if (requerimientosEliminar is not null && requerimientosEliminar.Any())
                {
                    _repository.DeleteRange(requerimientosEliminar);
                    _logger.LogInformation($"Se eliminaron {requerimientosEliminar.Count()} requerimientos.");
                }

                var nuevosRequerimientos = await RequerimientosParaActualizar(requerimiento);

                if (nuevosRequerimientos is not null && nuevosRequerimientos.Any())
                {
                    _repository.InsertRange(nuevosRequerimientos);
                }

                await _eventosService.Update(evento);
                await _repository.SaveChangeAsync();
                _logger.LogInformation("Proceso de actualización de requerimientos completado correctamente.");
                return true;

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
                var requerimientos = new List<Requerimientos>();

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

                var requisitosRequest = new
                {
                    Requisitos = requisitosActivos?.Select(m => m.Id)?.ToList(),
                    RequisitosCount = requisitosActivos?.Count()
                };

                var choferesRequest = new
                {
                    Choferes = choferesActivos?.Select(m => m.Id)?.ToList(),
                    ChoferesCount = choferesActivos?.Count()
                };


                var presentaciones = await _query
                                           .From<RequisitosPresentados>()
                                           .Where(rp => $"(0 = @RequisitosCount OR RequisitosPresentados.Requisito IN @Requisitos)", requisitosRequest, TypeWhere.AND)
                                           .Where(rp => $" (0 = @ChoferesCount OR RequisitosPresentados.Chofer IN @Choferes)", choferesRequest)
                                           .Execute();


                foreach (var chofer in choferesActivos)
                {
                    foreach (var requisito in requisitosActivos)
                    {
                        var requerimiento = new Requerimientos()
                        {
                            Chofer = chofer.Id,
                            Requisito = requisito.Id,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = requerimientoDto.Usuario,
                        };

                        _logger.LogInformation($"Creando requerimiento para chofer {chofer.Id} y requisito {requisito.Id}");

                        var presentacionesRequisito = presentaciones.Where(m => m.Requisito == requisito.Id).ToList();

                        PresentacionesHelper.DeterminarMejorPresentacion(presentacionesRequisito, requerimientos, requerimiento, requisito);
                    }
                }

                return requerimientos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar 'RequerimientosParaActualizar'");
                throw; 
            }
        }

    }
}
