using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using Domain.Entities.Presentacion;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces.Evento;
using gestion_transportista_cron_requerimientos.Application.Helpers;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Andreani.ARQ.Core.Clases.Enums;

namespace gestion_transportista_cron_requerimientos.Application.Strategies.Implementations.Documentos;

public class UnidadStrategy : IEntidadStrategy
{
    private readonly ITransactionalRepository _repository;
    private readonly IReadOnlyQuery _query;
    private readonly ILogger _logger;
    private readonly IEventosService _eventosService;

    public UnidadStrategy(
        IReadOnlyQuery query,
        ITransactionalRepository transactionalRepository,
        ILogger logger,
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
                                                .Where(r => $"(@Requisito IS NOT NULL AND @Unidad IS NOT NULL AND (Requisito = @Requisito AND Unidad = @Unidad))", new { requerimiento.Requisito, requerimiento.Unidad }, TypeWhere.OR)
                                                .Where(r => $"(@Requisito IS NOT NULL AND @Unidad IS NULL AND (Requisito = @Requisito))", new { requerimiento.Requisito }, TypeWhere.OR)
                                                .Where(r => $"(@Requisito IS NULL AND @Unidad IS NOT NULL AND (Unidad = @Unidad))", new { requerimiento.Unidad }, TypeWhere.OR)
                                                .Execute();


            if (requerimientosEliminar is not null && requerimientosEliminar.Any())
            {
                _repository.DeleteRange(requerimientosEliminar);
                _logger.Information($"Se eliminaron {requerimientosEliminar.Count()} requerimientos.");
            }

            var nuevosRequerimientos = await RequerimientosParaActualizar(requerimiento);

            if (nuevosRequerimientos is not null && nuevosRequerimientos.Any())
            {
                _repository.InsertRange(nuevosRequerimientos);
            }

            evento.FechaHoraPublicado = DateTime.Now;
            await _eventosService.Update(evento);
            await _repository.SaveChangeAsync();
            _logger.Information("Proceso de actualización de requerimientos completado correctamente.");
            return true;

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al actualizar los requerimientos.");
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
                                        .Where(r => $"Activo = 1 AND Entidad = 'U'", where: TypeWhere.AND)
                                        .Where(r => $"((@Requisito IS NOT NULL AND Id = @Requisito) OR @Requisito IS NULL)", new { requerimientoDto.Requisito })
                                        .Execute() ?? new List<Requisitos>();

            var unidadesActivas = await _query
                                        .From<Unidades>()
                                        .Where(r => $"Activo = 1", where: TypeWhere.AND)
                                        .Where(r => $"((@Unidad IS NOT NULL AND Id = @Unidad) OR @Unidad IS NULL)", new { requerimientoDto.Unidad })
                                        .Execute() ?? new List<Unidades>();

            var requisitosRequest = new
            {
                Requisitos = requisitosActivos?.Select(m => m.Id)?.ToList(),
                RequisitosCount = requisitosActivos?.Count()
            };

            var unidadesRequest = new
            {
                Unidades = unidadesActivas?.Select(m => m.Id)?.ToList(),
                UnidadesCount = unidadesActivas?.Count()
            };


            var presentaciones = await _query
                                       .From<RequisitosPresentados>()
                                       .Where(rp => $"(0 = @RequisitosCount OR RequisitosPresentados.Requisito IN @Requisitos)", requisitosRequest, TypeWhere.AND)
                                       .Where(rp => $" (0 = @UnidadesCount OR RequisitosPresentados.Unidad IN @Unidades)", unidadesRequest)
                                       .Execute() ?? new List<RequisitosPresentados>();

            var caracteristicasUnidades = await _query
                                       .From<CaracteristicasUnidades>()
                                       .Where(rp => $" (0 = @UnidadesCount OR Unidad IN @Unidades)", unidadesRequest)
                                       .Execute() ?? new List<CaracteristicasUnidades>();



            foreach (var unidad in unidadesActivas)
            {
                foreach (var requisito in requisitosActivos)
                {
                    var requerimiento = new Requerimientos()
                    {
                        Unidad = unidad.Id,
                        Requisito = requisito.Id,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = requerimientoDto?.UsuarioCreacion ?? "",
                    };

                    

                    var aplicaProveedor = caracteristicasUnidades.Any(m => m.Unidad == unidad.Id && m.Caracteristica == requisito.Caracteristica);

                    if (requisito.Caracteristica is null || (requisito.Caracteristica is not null && aplicaProveedor))
                    {
                        _logger.Information($"Creando requerimiento para unidad {unidad.Id} y requisito {requisito.Id}");

                        var presentacionesRequisito = presentaciones.Where(m => m.Requisito == requisito.Id).ToList();

                        PresentacionesHelper.DeterminarMejorPresentacion(presentacionesRequisito, requerimientos, requerimiento, requisito);
                    }
                }
            }

            return requerimientos;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error al ejecutar 'RequerimientosParaActualizar'");
            throw;
        }
    }
}
