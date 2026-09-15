namespace GesMgmt.Domain.Entities.Analitica;

public sealed record AnaliticaAlcanceReporteClienteMapeo
{
    public int IdClienteCrm { get; init; }
    public required string ClienteReporte { get; init; }
    public int IdGrupoSisges { get; init; }
}
