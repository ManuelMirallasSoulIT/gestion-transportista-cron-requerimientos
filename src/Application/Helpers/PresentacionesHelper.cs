using Domain.Entities.Presentacion;
using GDT.Common.Domain.Entities;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gestion_transportista_cron_requerimientos.Application.Helpers;

public static class PresentacionesHelper
{
    /// <summary>
    /// Determina la mejor presentación segun el tipo de vencimiento del requisito
    /// </summary>
    /// <param name="presentacionesRequisito"></param>
    /// <param name="requerimientos"></param>
    /// <param name="requerimiento"></param>
    public static void DeterminarMejorPresentacion(
        List<RequisitosPresentados> presentacionesRequisito, 
        List<Requerimientos> requerimientos,
        Requerimientos requerimiento,
        Requisitos requisito)
    {
        if(requisito.TipoVencimiento == "U")
        {
            MejorPresentacionUnicaVez(presentacionesRequisito, requerimientos, requerimiento);
            return;
        }

        MejorPresentacionVencimiento(presentacionesRequisito, requerimientos, requerimiento, requisito);
    }

    public static void MejorPresentacionUnicaVez(List<RequisitosPresentados> presentacionesRequisito,
        List<Requerimientos> requerimientos,
        Requerimientos requerimiento)
    {
        // Si no hay ninguna Presentación, devuelve Estado No Presentado y el Id de la Presentación nulo.
        if (!presentacionesRequisito.Any())
        {
            requerimiento.Estado = "No presentado";
            requerimiento.Presentacion = null;
            requerimientos.Add(requerimiento);
            return;
        }

        // Si no, si hay por lo menos una Presentación con estado Aprobado, devuelve Estado Aprobado y el mayor Id de la Presentación.
        if (presentacionesRequisito.Exists(m => m.Status == "A"))
        {
            requerimiento.Estado = "Aprobado";
            requerimiento.Presentacion = presentacionesRequisito.OrderByDescending(m => m.Id).First(m => m.Status == "A")?.Id;
            requerimientos.Add(requerimiento);
            return;
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
            requerimiento.Presentacion = presentacionesRequisito.OrderByDescending(m => m.Id).First(m => m.Status == "P")?.Id;
            requerimientos.Add(requerimiento);
            return;
        }

        // Si no, devuelve Estado Rechazado y el mayor Id de la Presentación.
        requerimiento.Estado = "Rechazado";
        requerimiento.Presentacion = presentacionesRequisito.OrderByDescending(m => m.Id).First().Id;
        requerimientos.Add(requerimiento);
    }

    public static void MejorPresentacionVencimiento(List<RequisitosPresentados> presentacionesRequisito,
        List<Requerimientos> requerimientos,
        Requerimientos requerimiento,
        Requisitos requisito)
    {

        // Si no hay ninguna Presentación, devuelve Estado No Presentado y el Id de la Presentación nulo.
        if (!presentacionesRequisito.Any())
        {
            requerimiento.Estado = "No presentado";
            requerimiento.Presentacion = null;
            requerimientos.Add(requerimiento);
            return;
        }

        // Si no, si hay por lo menos una Presentación no vencida con estado Aprobado, devuelve Estado Aprobado y el mayor Id de la Presentación.
        if (presentacionesRequisito.Exists(m => m.Status == "A" && m.Vencimiento <= DateTime.Now.AddDays(requisito.Tolerancia)))
        {
            requerimiento.Estado = "Aprobado";
            requerimiento.Presentacion = presentacionesRequisito.OrderByDescending(m => m.Id).First(m => m.Status == "A")?.Id;
            requerimientos.Add(requerimiento);
            return;
        }

        // TODO: Cuando se agregue la excepcion a requisito, validar: 
        // Si no, si hay por lo menos una Presentación no vencida con estado Aprobado por Excepción, devuelve Estado Aprobado por Excepción y el mayor Id de la Presentación.
        //if (presentacionesRequisito.Exists(m => m.Status == "A" ))
        //{

        //}

        // Si no, si hay por lo menos una Presentación no vencida con estado Pendiente, devuelve Estado Pendiente y el mayor Id de la Presentación.
        if (presentacionesRequisito.Exists(m => m.Status == "P" && m.Vencimiento <= DateTime.Now.AddDays(requisito.Tolerancia)))
        {
            requerimiento.Estado = "Pendiente";
            requerimiento.Presentacion = presentacionesRequisito.OrderByDescending(m => m.Id).First(m => m.Status == "P")?.Id;
            requerimientos.Add(requerimiento);
            return;
        }

        // Si no, si hay por lo menos una Presentación no vencida con estado Aprobado, devuelve Estado Aprobado y el mayor Id de la Presentación.
        if (presentacionesRequisito.Exists(m => m.Status == "A" && m.Vencimiento >= DateTime.Now.AddDays(requisito.Tolerancia)))
        {
            requerimiento.Estado = "Vencido";
            requerimiento.Presentacion = presentacionesRequisito.OrderByDescending(m => m.Id).First(m => m.Status == "A")?.Id;
            requerimientos.Add(requerimiento);
            return;
        }

        requerimiento.Estado = "Rechazado";
        requerimiento.Presentacion = presentacionesRequisito.OrderByDescending(m => m.Id).First().Id;
        requerimientos.Add(requerimiento);
    }
}
