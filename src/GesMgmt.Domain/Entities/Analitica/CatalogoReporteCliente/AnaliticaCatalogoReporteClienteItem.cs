namespace GesMgmt.Domain.Entities.Analitica;

public sealed record AnaliticaCatalogoReporteClienteItem
{
    public int IdClienteCrm { get; init; }
    public required string ClienteReporte { get; init; }
}
