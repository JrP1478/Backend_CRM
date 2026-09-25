using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record ActualizarAnaliticaIncrustacionReporteClienteOpcion(
    int IdCliente,
    string Nombre,
    IReadOnlyList<int>? IdsGrupos,
    string? UrlIncrustacion);
