using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record ActualizarAnaliticaConfiguracionPowerBiRequest(
    string? CodigoOpcion,
    string? NombreOpcion,
    bool EsActivo,
    IReadOnlyList<int>? IdsGrupos,
    IReadOnlyList<ActualizarAnaliticaIncrustacionReporteClienteOpcion>? Publicaciones);
