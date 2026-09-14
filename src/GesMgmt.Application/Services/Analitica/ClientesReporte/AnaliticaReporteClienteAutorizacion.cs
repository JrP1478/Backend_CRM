using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public static class AnaliticaReporteClienteAutorizacion
{
    public static bool EstaAutorizado(
        AnaliticaConfiguracionReporteCliente configuration,
        IReadOnlySet<int> activeUserGroupIds) =>
        configuration.EstaLista &&
        configuration.IdsGrupos.Count > 0 &&
        configuration.IdsGrupos.All(activeUserGroupIds.Contains);

    public static bool Coincide(
        AnaliticaConfiguracionReporteCliente configuration,
        int idCliente,
        string reportClient) =>
        configuration.IdCliente == idCliente &&
        string.Equals(
            configuration.Nombre.Trim(),
            reportClient.Trim(),
            StringComparison.OrdinalIgnoreCase);
}
