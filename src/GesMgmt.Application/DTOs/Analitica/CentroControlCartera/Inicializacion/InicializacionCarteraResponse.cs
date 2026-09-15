namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record InicializacionCarteraResponse(
    OpcionesFiltroCarteraResponse FilterOptions,
    PanoramaCarteraResponse? Panorama);
