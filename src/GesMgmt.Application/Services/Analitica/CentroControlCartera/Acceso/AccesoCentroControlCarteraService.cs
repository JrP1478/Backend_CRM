using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class AccesoCentroControlCarteraService(
    IAccesoAnaliticaService analyticsAccess,
    IContextoUsuarioAnalitica userContext)
    : IAccesoCentroControlCarteraService
{
    public async Task<CentroControlCarteraClienteAcceso> ResolverClienteAsync(
        int? requestedCrmClientId,
        CancellationToken cancellationToken)
    {
        using var phase = DiagnosticosCentroControlCartera.IniciarFase(
            DiagnosticosCentroControlCartera.FaseAcceso);

        if (requestedCrmClientId is <= 0)
        {
            return CentroControlCarteraClienteAcceso.Error(
                400,
                "Cartera Analítica inválida",
                "idClienteCrm debe ser un entero positivo.");
        }

        if (!userContext.IntentarObtenerIdUsuario(out var idUsuario))
        {
            return CentroControlCarteraClienteAcceso.Error(
                401,
                "Identidad Analítica no disponible",
                "No se pudo determinar el usuario CRM para validar el acceso a Analítica.");
        }

        if (requestedCrmClientId.HasValue)
        {
            var isAllowed = await analyticsAccess.EstaClientePermitidoAsync(
                idUsuario,
                AnaliticaOpcionIds.CentroControlCartera,
                requestedCrmClientId.Value,
                cancellationToken);

            if (!isAllowed)
            {
                return CentroControlCarteraClienteAcceso.Error(
                    403,
                    "Cartera Analítica no autorizada",
                    $"El usuario no tiene acceso a la cartera CRM '{requestedCrmClientId.Value}'.");
            }

            return CentroControlCarteraClienteAcceso.Permitido(
                requestedCrmClientId.Value);
        }

        var allowedClientIds = await analyticsAccess.ObtenerIdsClientesPermitidosAsync(
            idUsuario,
            AnaliticaOpcionIds.CentroControlCartera,
            cancellationToken);

        if (allowedClientIds.Count == 0)
        {
            return CentroControlCarteraClienteAcceso.Error(
                403,
                "Sin acceso a Centro de Control de Cartera",
                "El usuario no tiene carteras autorizadas para Centro de Control de Cartera.");
        }

        return CentroControlCarteraClienteAcceso.Permitido(
            allowedClientIds.OrderBy(x => x).First());
    }
}
