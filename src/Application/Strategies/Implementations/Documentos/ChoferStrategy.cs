
using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using Domain.Entities.Presentaciones;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using StackExchange.Redis;
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

        public ChoferStrategy(IReadOnlyQuery query, ITransactionalRepository transactionalRepository, IApplicationDbContext context)
        {
            _query = query;
            _transactionalRepository = transactionalRepository;
            _context = context;
        }

        public async Task<bool> ActualizarRequerimientos(RequerimientoDto requerimiento)
        {
            return await _context.CreateTransaction(async context =>
            {
                var requerimientosEliminar = await _query
                    .From<Requerimientos>()
                    .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NOT NULL AND (Requisito = @Requisito AND Chofer = @Chofer))", new { requerimiento.Requisito, requerimiento.Chofer }, TypeWhere.OR)
                    .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NULL AND (Requisito = @Requisito))", new { requerimiento.Requisito }, TypeWhere.OR)
                    .Where(r => $"(@Requisito IS NULL AND @Chofer IS NOT NULL AND (Chofer = @Chofer))", new { requerimiento.Chofer }, TypeWhere.OR)
                    .Execute();
                _transactionalRepository.DeleteRange(requerimientosEliminar);

                await RequerimientosParaActualizar(requerimiento);
                //_transactionalRepository.InsertRange(requerimientosInsertar);
            });
        }

        private async Task RequerimientosParaActualizar(RequerimientoDto requerimientoDto)
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
                                           .From<RequisitoPresentado>()
                                           .Where(rp => $"", new { }, TypeWhere.AND)
                                           .Where(rp => $"", new { })
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

                    var presentacionesRequisito = presentaciones.Where(m => m.Requisito.Id == requisito.Id).ToList();

                    // Si no hay ninguna Presentación, devuelve Estado No Presentado y el Id de la Presentación nulo.
                    if (!presentacionesRequisito.Any())
                    {
                        requerimiento.Estado = "No presentado";
                        requerimiento.Presentacion = null;
                        continue;
                    }

                    // Si no, si hay por lo menos una Presentación con estado Aprobado, devuelve Estado Aprobado y el mayor Id de la Presentación.
                    if (presentacionesRequisito.Exists(m => m.Status == "A"))
                    {
                        requerimiento.Estado = "Aprobado";
                        requerimiento.Presentacion = presentaciones.OrderByDescending(m => m.Id).First(m => m.Status == "A")?.Id;
                        continue;
                    }

                    // TODO: Cuando se agregue la excepcion a requisito, validar: 
                    // Si no, si hay por lo menos una Presentación con estado Aprobado por Excepción, devuelve Estado Aprobado por Excepción y el mayor Id de la Presentación.
                    //if (presentacionesRequisito.Exists(m => m.Status == "A" ))
                    //{

                    //}

                    // Si no, si hay por lo menos una Presentación con estado Pendiente, devuelve Estado Pendiente y el mayor Id de la Presentación.
                    if (presentacionesRequisito.Exists(m => m.Status == "P"))
                    {
                        requerimiento.Estado = "Pendiente";
                        requerimiento.Presentacion = presentaciones.OrderByDescending(m => m.Id).First(m => m.Status == "P")?.Id;
                        continue;
                    }

                    // Si no, devuelve Estado Rechazado y el mayor Id de la Presentación.
                    requerimiento.Estado = "Rechazado";
                    requerimiento.Presentacion = presentaciones.OrderByDescending(m => m.Id).First().Id;

                }
            }
        }


    }
}
