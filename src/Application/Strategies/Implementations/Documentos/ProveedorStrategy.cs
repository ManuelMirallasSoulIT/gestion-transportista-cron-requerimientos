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

public class ProveedorStrategy : IEntidadStrategy
{
    private readonly ITransactionalRepository _repository;
    private readonly IReadOnlyQuery _query;
    private readonly ILogger _logger;
    private readonly IEventosService _eventosService;

    public ProveedorStrategy(
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
                                                .Where(r => $"(@Requisito IS NOT NULL AND @Proveedor IS NOT NULL AND (Requisito = @Requisito AND Proveedor = @Proveedor))", new { requerimiento.Requisito, requerimiento.Proveedor }, TypeWhere.OR)
                                                .Where(r => $"(@Requisito IS NOT NULL AND @Proveedor IS NULL AND (Requisito = @Requisito))", new { requerimiento.Requisito }, TypeWhere.OR)
                                                .Where(r => $"(@Requisito IS NULL AND @Proveedor IS NOT NULL AND (Proveedor = @Proveedor))", new { requerimiento.Proveedor }, TypeWhere.OR)
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
                                        .Where(r => $"Activo = 1 AND Entidad = 'P'", where: TypeWhere.AND)
                                        .Where(r => $"((@Requisito IS NOT NULL AND Id = @Requisito) OR @Requisito IS NULL)", new { requerimientoDto.Requisito })
                                        .Execute();

            var proveedoresActivos = await _query
                                        .From<Proveedores>()
                                        .Where(r => $"Activo = 1", where: TypeWhere.AND)
                                        .Where(r => $"((@Proveedor IS NOT NULL AND Id = @Proveedor) OR @Proveedor IS NULL)", new { requerimientoDto.Proveedor })
                                        .Execute();

            var requisitosRequest = new
            {
                Requisitos = requisitosActivos?.Select(m => m.Id)?.ToList(),
                RequisitosCount = requisitosActivos?.Count()
            };

            var proveedoresRequest = new
            {
                Proveedores = proveedoresActivos?.Select(m => m.Id)?.ToList(),
                ProveedoresCount = proveedoresActivos?.Count()
            };


            var presentaciones = await _query
                                       .From<RequisitosPresentados>()
                                       .Where(rp => $"(0 = @RequisitosCount OR RequisitosPresentados.Requisito IN @Requisitos)", requisitosRequest, TypeWhere.AND)
                                       .Where(rp => $" (0 = @ProveedoresCount OR RequisitosPresentados.Proveedor IN @Proveedores)", proveedoresRequest)
                                       .Execute();

            var caracteristicasProveedores = await _query
                                       .From<CaracteristicasProveedores>()
                                       .Where(rp => $" (0 = @ProveedoresCount OR Proveedor IN @Proveedores)", proveedoresRequest)
                                       .Execute();



            foreach (var proveedor in proveedoresActivos)
            {
                foreach (var requisito in requisitosActivos)
                {
                    var requerimiento = new Requerimientos()
                    {
                        Proveedor = proveedor.Id,
                        Requisito = requisito.Id,
                        FechaCreacion = DateTime.Now,
                        UsuarioCreacion = requerimientoDto?.Usuario ?? "",
                    };

                    _logger.Information($"Creando requerimiento para proveedor {proveedor.Id} y requisito {requisito.Id}");

                    // Si es distinto de null, aplica
                    var caracteristica = caracteristicasProveedores.FirstOrDefault(m => requisito.Caracteristica != null &&
                                                                            m.Caracteristica == requisito.Caracteristica &&
                                                                            m.Proveedor == proveedor.Id);

                    var presentacionesRequisito = presentaciones.Where(m => m.Requisito == requisito.Id).ToList();

                    PresentacionesHelper.DeterminarMejorPresentacion(presentacionesRequisito, requerimientos, requerimiento, requisito);
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
