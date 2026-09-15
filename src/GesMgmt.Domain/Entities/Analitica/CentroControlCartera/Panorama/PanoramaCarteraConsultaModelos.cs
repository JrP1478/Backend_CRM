
namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record PanoramaCarteraContexto(
    ResumenCarteraContexto Resumen,
    bool SubCarteraOperativaDisponible);

public sealed record PanoramaCarteraDbFilas(
    ResumenCarteraDbFila Resumen,
    AvanceMetaCarteraDbFila? TargetProgress,
    PromesasCarteraDbFila Promises,
    IReadOnlyList<EvolucionCarteraDbFila> Evolucion);
