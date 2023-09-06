using GDT.Common.Domain.Entities;

namespace gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;

public class CaracteristicasUnidades
{
    public long Caracteristica { get; set; }
    public long Unidad { get; set; }

    public virtual Unidades UnidadDto { get; set; }

    public virtual Caracteristicas CaracteristicaDto { get; set; }
}
