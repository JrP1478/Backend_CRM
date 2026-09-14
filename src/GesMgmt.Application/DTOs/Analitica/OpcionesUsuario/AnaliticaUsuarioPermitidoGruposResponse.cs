using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaUsuarioPermitidoGruposResponse(
    int IdOpcion,
    bool Permitido,
    string ModoAlcance,
    IReadOnlyList<int> IdsGrupos,
    bool RequiereSeleccionCliente);
