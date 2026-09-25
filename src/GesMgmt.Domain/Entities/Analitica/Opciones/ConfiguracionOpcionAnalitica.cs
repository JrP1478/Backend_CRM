namespace GesMgmt.Domain.Entities.Analitica;

public sealed class ConfiguracionOpcionAnalitica
{
    public int IdOpcion { get; set; }
    public string CodigoOpcion { get; set; } = string.Empty;
    public string NombreOpcion { get; set; } = string.Empty;
    public bool EsActivo { get; set; }
    public int? CreadoPor { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int? ActualizadoPor { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
