using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record ActualizarAnaliticaUsuarioOpcionesRequest(
    IReadOnlyCollection<int>? IdsUsuarios);
