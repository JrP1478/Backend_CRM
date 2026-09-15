using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record ActualizarAnaliticaGruposOpcionRequest(
    IReadOnlyList<int>? IdsGrupos);
