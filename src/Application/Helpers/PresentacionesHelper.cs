using Domain.Entities.Presentacion;
using gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;
using System.Collections.Generic;
using System.Linq;

namespace gestion_transportista_cron_requerimientos.Application.Helpers;

public static class PresentacionesHelper
{
    /// <summary>
    /// Determina la mejor presentación
    /// </summary>
    /// <param name="presentacionesRequisito"></param>
    /// <param name="requerimientos"></param>
    /// <param name="requerimiento"></param>
    public static void DeterminateBetterPresentation(
        List<RequisitosPresentados> presentacionesRequisito, 
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
}
