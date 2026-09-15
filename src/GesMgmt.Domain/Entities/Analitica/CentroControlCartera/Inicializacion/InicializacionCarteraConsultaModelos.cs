
namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record InicializacionCarteraOrigen(
    OpcionesFiltroCarteraDbResult FilterOptions,
    PanoramaCarteraContexto? PanoramaContext);

public sealed record InicializacionCarteraPanoramaContextoDbFila
{
    public int ClaveCliente { get; init; }
    public int ClaveCampana { get; init; }
    public required string CodigoCampana { get; init; }
    public required string NombreCampana { get; init; }
    public DateTime FechaInicio { get; init; }
    public DateTime FechaFin { get; init; }
    public DateTime? FechaUltimoDato { get; init; }
    public bool SubCarteraOperativaDisponible { get; init; }
}
