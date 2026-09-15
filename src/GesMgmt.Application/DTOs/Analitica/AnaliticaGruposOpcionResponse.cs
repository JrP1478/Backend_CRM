using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaGruposOpcionResponse(
    int IdOpcion,
    IReadOnlyList<int> IdsGrupos);
