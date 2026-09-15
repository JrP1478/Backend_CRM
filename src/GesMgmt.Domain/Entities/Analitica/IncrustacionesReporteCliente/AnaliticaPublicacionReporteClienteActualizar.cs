namespace GesMgmt.Domain.Entities.Analitica;

public sealed record AnaliticaPublicacionReporteClienteActualizar(
    int IdCliente,
    string Nombre,
    IReadOnlyCollection<int>? IdsGrupos,
    string? UrlIncrustacion);
