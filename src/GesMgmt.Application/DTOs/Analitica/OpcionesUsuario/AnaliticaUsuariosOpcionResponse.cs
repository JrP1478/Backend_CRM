using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaUsuariosOpcionResponse(
    int IdOpcion,
    IReadOnlyList<int> IdsUsuarios);
