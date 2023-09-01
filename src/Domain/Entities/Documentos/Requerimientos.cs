using System;

namespace gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;

public class Requerimientos
{
    public long Id { get; set; }
    public long? Requisito { get; set; }
    public long? Chofer { get; set; }
    public long? Proveedor { get; set; }
    public long? Unidad { get; set; }
    public string Estado { get; set; }
    public long? Presentacion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public string UsuarioCreacion { get; set; }
    public string UsuarioModificacion { get; set; }
}
