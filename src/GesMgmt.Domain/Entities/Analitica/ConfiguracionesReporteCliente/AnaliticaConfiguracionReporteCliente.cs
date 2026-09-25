namespace GesMgmt.Domain.Entities.Analitica;

public sealed record AnaliticaConfiguracionReporteCliente(
    int IdCliente,
    string Nombre,
    bool EstaDisponible,
    string GroupResolution,
    bool TieneConfiguracionGruposExplicita,
    IReadOnlyList<int> IdsGrupos,
    IReadOnlyList<AnaliticaConfiguracionReporteClienteGrupo> GruposCandidatos,
    string? UrlIncrustacion,
    bool EstaLista);
