
using Andreani.ARQ.Core.Interface;
using Application.Strategies;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Application.Common.Interfaces;
using gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
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
                var requerimientosEliminar = await RequerimientosParaEliminar(requerimiento);
                _transactionalRepository.DeleteRange(requerimientosEliminar);

                var RequerimientosInsertar = await RequerimientosParaActualizar(requerimiento);
                _transactionalRepository.InsertRange(RequerimientosInsertar);
            });
        }

        private async Task<IEnumerable<Requerimientos>> RequerimientosParaEliminar(RequerimientoDto requerimiento)
        {
            var requerimientos = await _query
                    .From<Requerimientos>()
                    .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NOT NULL AND (Requisito = @Requisito AND Chofer = @Chofer))", new { requerimiento.Requisito, requerimiento.Chofer }, TypeWhere.OR)
                    .Where(r => $"(@Requisito IS NOT NULL AND @Chofer IS NULL AND (Requisito = @Requisito))", new { requerimiento.Requisito }, TypeWhere.OR)
                    .Where(r => $"(@Requisito IS NULL AND @Chofer IS NOT NULL AND (Chofer = @Chofer))", new { requerimiento.Chofer }, TypeWhere.OR)
                    .Execute();

            return requerimientos;
        }

        private async Task<IEnumerable<Requerimientos>> RequerimientosParaActualizar(RequerimientoDto requerimiento)
        {
            var nuevosRequerimientos = new List<Requerimientos>();

            var requisitosActivos = await _query.From<Requisitos>().Where(r => r.Activo).Execute();
            var choferesActivos = await _query.From<Choferes>().Where(c => c.Activo).Execute();

            var requisitosProcesar = requerimiento.Requisito.HasValue
                ? requisitosActivos.Where(r => r.Id == requerimiento.Requisito.Value)
                : requisitosActivos;

            foreach (var requisito in requisitosProcesar)
            {
                await ProcesarRequisito(requisito, requerimiento.Chofer, choferesActivos, nuevosRequerimientos);
            }

            return nuevosRequerimientos;
        }


        private async Task ProcesarRequisito(Requisito requisito, long choferId)
        {
            if (!requisito.AplicaAChofer) return;

            var choferesActivos = await _query.From<Choferes>().Where(c => c.Activo).Execute();

            foreach (var chofer in choferesActivos)
            {
                await CrearRequerimiento(requisito, chofer);
            }
        }
        

        private bool cumpleCaracteristica(Choferes chofer, Caracteristicas caracteristica)
        {
            //// COMO (/&%$ DETERMINO CARACTERISTICAS A CUMPLIR ?????????????

            return false;
        }

        private async Task CrearRequerimiento(Requisito requisito, Choferes chofer)
        {
            if (requisito.Caracteristica == null || cumpleCaracteristica(chofer, requisito.Caracteristica))
            {
                var mejorPresentacion = await ObtenerMejorPresentacion(requisito, chofer);

                var nuevoRequerimiento = new Requerimientos
                {
                    Requisito = requisito.Id,
                    Proveedor = requisito.Proveedor,
                    Chofer = chofer.Id,
                    Unidad = chofer.Unidad,
                    Estado = CalcularEstadoRequerimiento(mejorPresentacion),
                    Presentacion = mejorPresentacion?.Id ?? 0,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    UsuarioCreacion = "",
                    UsuarioModificacion = null
                };

                _transactionalRepository.Insert(nuevoRequerimiento);
            }
        }       

        /////////////////////////////////// Opcion 1
        private async Task<Presentacion> ObtenerMejorPresentacion(Requisito requisito, Choferes chofer)
        {
            var presentaciones = await _query
                .From<requisitosPresentados>()
                .Where(p => p.Requisito == requisito.Id && p.Proveedor == requisito.Proveedor && p.Chofer == chofer.Id && p.Unidad == chofer.Unidad)
                .Execute();

            if (!presentaciones.Any()) return null;
            

            var presentacionesOrdenadas = requisito.TipoVencimiento switch
            {
                TipoVencimiento.UnicaVez => presentaciones.OrderBy(p => p.Estado == EstadoPresentacion.Aprobado ? 0 : p.Estado == EstadoPresentacion.AprobadoPorExcepcion ? 1 : 2)
                                                         .ThenBy(p => p.Estado == EstadoPresentacion.Aprobado ? p.Id : p.Id)
                                                         .FirstOrDefault(),
                TipoVencimiento.Vencimiento => presentaciones.OrderBy(p => p.Estado == EstadoPresentacion.Aprobado ? 0 : p.Estado == EstadoPresentacion.AprobadoPorExcepcion ? 1 : p.FechaVencimiento < DateTime.Now ? 4 : 2)
                                                             .ThenBy(p => p.Estado == EstadoPresentacion.Aprobado && p.FechaVencimiento < DateTime.Now ? -p.Id : p.Id)
                                                             .FirstOrDefault(),
              _ => throw new ArgumentException($"Tipo de vencimiento no reconocido: {tipoVencimientoDesconocido}")
              
            };

            return presentacionesOrdenadas;
        }
        ////////////////// OPCION 2
        private async Task<Presentacion> ObtenerMejorPresentacion1(Requisito requisito, Choferes chofer)
        {
            var presentaciones = await _query
                .From<requisitosPresentados>()
                .Where(p => p.Requisito == requisito.Id && p.Proveedor == requisito.Proveedor && p.Chofer == chofer.Id && p.Unidad == chofer.Unidad)
                .Execute();

            if (!presentaciones.Any()) return null;

            var presentacionesOrdenadas = requisito.TipoVencimiento switch
            {
                TipoVencimiento.UnicaVez => OrdenarPresentacionesUnicaVez(presentaciones),
                TipoVencimiento.Vencimiento => OrdenarPresentacionesVencimiento(presentaciones),
                _ => throw new ArgumentException($"Tipo de vencimiento no reconocido: {tipoVencimientoDesconocido}")
            };

            return presentacionesOrdenadas;
        }

        private Presentacion OrdenarPresentacionesUnicaVez(IEnumerable<Presentacion> presentaciones)
        {
            return presentaciones.OrderBy(p =>
                    p.Estado == EstadoPresentacion.Aprobado ? 0 :
                    p.Estado == EstadoPresentacion.AprobadoPorExcepcion ? 1 : 2)
                .ThenBy(p => p.Estado == EstadoPresentacion.Aprobado ? p.Id : p.Id)
                .FirstOrDefault();
        }

        private Presentacion OrdenarPresentacionesVencimiento(IEnumerable<Presentacion> presentaciones)
        {
            return presentaciones.OrderBy(p =>
                    p.Estado == EstadoPresentacion.Aprobado ? 0 :
                    p.Estado == EstadoPresentacion.AprobadoPorExcepcion ? 1 :
                    p.FechaVencimiento < DateTime.Now ? 4 : 2)
                .ThenBy(p =>
                    p.Estado == EstadoPresentacion.Aprobado && p.FechaVencimiento < DateTime.Now ?
                    -p.Id : p.Id)
                .FirstOrDefault();
        }


        private EstadoRequerimiento CalcularEstadoRequerimiento(Presentacion presentacion)
        {
            return presentacion?.Estado switch
            {
                EstadoPresentacion.Aprobado => EstadoRequerimiento.Aprobado,
                EstadoPresentacion.AprobadoPorExcepcion => EstadoRequerimiento.AprobadoPorExcepcion,
                EstadoPresentacion.Pendiente => EstadoRequerimiento.Pendiente,
                EstadoPresentacion.Vencido => EstadoRequerimiento.Vencido,
                _ => EstadoRequerimiento.Rechazado
            };
        }


    }
}
