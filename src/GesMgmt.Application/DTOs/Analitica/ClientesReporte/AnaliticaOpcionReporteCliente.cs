using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaOpcionReporteCliente(
    int IdCliente,
    string Nombre);
