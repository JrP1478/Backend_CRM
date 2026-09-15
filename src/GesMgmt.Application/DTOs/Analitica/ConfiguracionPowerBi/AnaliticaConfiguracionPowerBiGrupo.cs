using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaConfiguracionPowerBiGrupo(
    int IdGrupo,
    int IdCliente,
    string Nombre);
