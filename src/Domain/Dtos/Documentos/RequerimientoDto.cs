namespace gestion_transportista_cron_requerimientos.Domain.Dtos.Documentos;

public class RequerimientoDto
{
    public long? Requisito { get; set; } = null;
    public long? Proveedor { get; set; } = null;
    public long? Unidad { get; set; } = null;
    public long? Chofer { get; set; } = null;
    public string Tipo { get; set; }
    public string Usuario { get; set; }
}
