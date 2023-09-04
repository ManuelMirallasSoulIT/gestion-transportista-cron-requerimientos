using GDT.Common.Domain.Entities;

namespace gestion_transportista_cron_requerimientos.Domain.Entities.Documentos;

public class CaracteristicasChoferes
{
    public long Caracteristica { get; set; }
    public long Chofer { get; set; }

    public virtual Choferes ChoferDto { get; set; }

    public virtual Caracteristicas CaracteristicaDto { get; set; }
}
