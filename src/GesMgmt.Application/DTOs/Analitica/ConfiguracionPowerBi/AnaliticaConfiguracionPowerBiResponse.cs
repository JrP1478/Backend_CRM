using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaConfiguracionPowerBiResponse(
    int IdOpcion,
    bool EstaConfigurada,
    IReadOnlyList<int> IdsGrupos,
    IReadOnlyList<AnaliticaConfiguracionPowerBiGrupo> AvailableGroups,
    IReadOnlyList<AnaliticaIncrustacionReporteClienteOpcion> Clientes);
