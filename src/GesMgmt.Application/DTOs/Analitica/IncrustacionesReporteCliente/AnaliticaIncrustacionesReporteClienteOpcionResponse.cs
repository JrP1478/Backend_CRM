using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaIncrustacionesReporteClienteOpcionResponse(
    int IdOpcion,
    IReadOnlyList<AnaliticaIncrustacionReporteClienteOpcion> Clientes);
