namespace GesMgmt.Domain.Entities.Analitica;

public sealed record AnaliticaIncrustacionReporteClienteMapeo
{
    public required string ClienteReporte { get; init; }
    public required string UrlIncrustacion { get; init; }
}
