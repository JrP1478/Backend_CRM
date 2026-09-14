namespace GesMgmt.Domain.Entities.Analitica;

public sealed class AnaliticaIncrustacionReporteClienteEntrada
{
    public int IdOpcion { get; set; }
    public int IdClienteCrm { get; set; }
    public string ClienteReporte { get; set; } = string.Empty;
    public string UrlIncrustacion { get; set; } = string.Empty;
    public bool EsActivo { get; set; }
    public int? CreadoPor { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int? ActualizadoPor { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
