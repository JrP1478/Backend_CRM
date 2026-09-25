namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record PanoramaCarteraResponse(
    ResumenCarteraResponse Resumen,
    AvanceMetaCarteraResponse TargetProgress,
    PromesasCarteraResponse Promises,
    EvolucionCarteraResponse Evolucion);
