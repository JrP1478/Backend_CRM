using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaClientesOpcionResponse(
    int IdOpcion,
    IReadOnlyList<int> IdsClientes);
