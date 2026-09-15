using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaIncrustacionReporteClienteResponse(
    int IdOpcion,
    int IdCliente,
    string Nombre,
    string UrlIncrustacion);
