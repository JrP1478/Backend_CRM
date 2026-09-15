namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record CarteraFiltroCampanaDbFila
{
    public required string CodigoCampana { get; init; }
    public required string NombreCampana { get; init; }
    public DateTime FechaInicio { get; init; }
    public DateTime FechaFin { get; init; }
    public DateTime FechaDisponibleDesde { get; init; }
    public DateTime FechaDisponibleHasta { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
public sealed record CarteraFiltroSubcarteraDbFila
{
    public long IdSubCartera { get; init; }
    public required string NombreSubCartera { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
public sealed record CarteraFiltroSubcarteraCampanaDbFila
{
    public long IdSubCartera { get; init; }
    public required string NombreSubCartera { get; init; }
    public string? CodigoUnidadNegocio { get; init; }
    public required string CodigoCampana { get; init; }
    public DateTime FechaDisponibleDesde { get; init; }
    public DateTime FechaDisponibleHasta { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
    public DateTime? FechaActualizacionSubCarteraUtc { get; init; }
    public long OrdenSubCartera { get; init; }
}
public sealed record CarteraFiltroSupervisorDbFila
{
    public int IdSupervisor { get; init; }
    public required string NombreSupervisor { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
public sealed record CarteraFiltroSupervisorContextoDbFila
{
    public int IdSupervisor { get; init; }
    public required string NombreSupervisor { get; init; }
    public long IdSubCartera { get; init; }
    public required string CodigoCampana { get; init; }
    public DateTime FechaDisponibleDesde { get; init; }
    public DateTime FechaDisponibleHasta { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
    public DateTime? FechaActualizacionSupervisorUtc { get; init; }
    public long OrdenSupervisor { get; init; }
}
public sealed record OpcionesFiltroCarteraDbResult(
    IReadOnlyList<CarteraFiltroCampanaDbFila> Campanas,
    IReadOnlyList<CarteraFiltroSubcarteraDbFila> SubPortfolios,
    IReadOnlyList<CarteraFiltroSubcarteraCampanaDbFila> SubPortfolioCampaigns,
    IReadOnlyList<CarteraFiltroSupervisorDbFila> Supervisores,
    IReadOnlyList<CarteraFiltroSupervisorContextoDbFila> SupervisorContexts)
{
    public IReadOnlyList<string> UnidadesNegocio { get; init; } = [];
}
