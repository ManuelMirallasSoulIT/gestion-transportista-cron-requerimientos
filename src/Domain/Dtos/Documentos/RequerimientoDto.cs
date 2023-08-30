namespace gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;

public class RequerimientoDto
{
    public long Requisito { get; set; }
    public long Proveedor { get; set; }
    public long Unidad { get; set; }
    public long Chofer { get; set; }
    public string Tipo { get; set; }
}
