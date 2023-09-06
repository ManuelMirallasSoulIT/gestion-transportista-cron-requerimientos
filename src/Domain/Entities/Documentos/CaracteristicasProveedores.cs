using GDT.Common.Domain.Entities;

namespace gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;

public class CaracteristicasProveedores
{
    public long Caracteristica { get; set; }
    public long Proveedor { get; set; }

    public virtual Proveedores ProveedorDto { get; set; }

    public virtual Caracteristicas CaracteristicaDto { get; set; }
}
