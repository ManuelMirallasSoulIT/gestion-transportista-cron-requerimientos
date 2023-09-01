using GDT.Common.Domain.Entities;
using System;

namespace Domain.Entities.Presentacion;

public class RequisitosPresentados
{
    public long Id { get; set; }
    public long Proveedor { get; set; }
    public long? Unidad { get; set; }
    public long? Chofer { get; set; }
    public long Requisito { get; set; }
    public DateTime? Vencimiento{ get; set; }
    public string Documento { get; set; }
    public string Status { get; set; }
    public DateTime? FechaPresentacion { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public string UsuarioAprobacion { get; set; }
    public string MotivoRechazo { get; set; }

    // Relantionship configurations

    public virtual Requisitos RequisitoDto { get; set; }
    public virtual Proveedores ProveedorDto { get; set; }
    public virtual Choferes ChoferDto { get; set; }
    public virtual Unidades UnidadDto { get; set; }
}
