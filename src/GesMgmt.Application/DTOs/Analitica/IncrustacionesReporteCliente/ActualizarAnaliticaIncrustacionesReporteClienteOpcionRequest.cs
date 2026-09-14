using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record ActualizarAnaliticaIncrustacionesReporteClienteOpcionRequest(
    IReadOnlyList<ActualizarAnaliticaIncrustacionReporteClienteOpcion>? Publicaciones);
