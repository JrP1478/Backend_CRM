using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaIncrustacionReporteClienteOpcion(
    int IdCliente,
    string Nombre,
    bool EstaDisponible,
    string GroupResolution,
    bool TieneConfiguracionGruposExplicita,
    IReadOnlyList<int> IdsGrupos,
    IReadOnlyList<AnaliticaOpcionReporteClienteGrupo> GruposCandidatos,
    string? UrlIncrustacion,
    bool EstaLista);
