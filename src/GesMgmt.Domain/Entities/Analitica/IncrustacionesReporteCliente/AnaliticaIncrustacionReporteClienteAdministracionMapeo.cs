namespace GesMgmt.Domain.Entities.Analitica;

public sealed record AnaliticaIncrustacionReporteClienteAdministracionMapeo
{
    public int IdClienteCrm { get; init; }
    public required string ClienteReporte { get; init; }
    public string? UrlIncrustacion { get; init; }
}
