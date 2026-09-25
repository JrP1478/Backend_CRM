using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaClientesReporteResponse(
    int IdOpcion,
    IReadOnlyList<AnaliticaOpcionReporteCliente> Clientes);
