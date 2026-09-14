using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class PromesasCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IPromesasCarteraRepository repository) : IPromesasCarteraService
{
    public async Task<CarteraOperacionResult<PromesasCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? fechaDesde,
        string? fechaHasta,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!PromesasCarteraRequest.IntentarCrear(
                campana,
                idSubCartera,
                fechaDesde,
                fechaHasta,
                out var request,
                out var requestErrors))
        {
            return CarteraOperacionResult<PromesasCarteraResponse>.Validacion(requestErrors);
        }

        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var normalizedBusinessUnit,
                out var businessUnitErrors))
        {
            return CarteraOperacionResult<PromesasCarteraResponse>.Validacion(businessUnitErrors);
        }

        var validRequest = request!;

        var clientAccess = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!clientAccess.EstaPermitido)
        {
            return CarteraOperacionResult<PromesasCarteraResponse>.DesdeAcceso(clientAccess);
        }

        var effectiveCrmClientId = clientAccess.IdClienteCrm!.Value;
        validRequest = validRequest with
        {
            UnidadNegocio = UnidadNegocioCarteraPolicy.ResolverSolicitadoOPredeterminado(
                effectiveCrmClientId,
                normalizedBusinessUnit)
        };

        var context = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseContexto,
            () => repository.ResolverContextoAsync(
                effectiveCrmClientId,
                validRequest.Campana,
                validRequest.IdSubCartera,
                validRequest.UnidadNegocio,
                cancellationToken));

        if (context is null)
        {
            return CarteraOperacionResult<PromesasCarteraResponse>.Problema(
                codigoEstado: 404,
                titulo: "Campaña Analítica no encontrada",
                detalle: validRequest.IdSubCartera is not null
                    ? validRequest.Campana is null
                        ? $"No existe una campaña disponible para la subcartera Analítica '{validRequest.IdSubCartera}' dentro del cliente autorizado."
                        : $"No existe la campaña '{validRequest.Campana}' asociada a la subcartera Analítica '{validRequest.IdSubCartera}' dentro del cliente autorizado."
                    : validRequest.Campana is null
                        ? "No existe una campaña disponible para el cliente autorizado."
                        : $"No existe la campaña '{validRequest.Campana}' para el cliente autorizado.");
        }

        var row = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseConsulta,
            () => repository.ObtenerPromesasOperacionalesAsync(
                context.ClaveCliente,
                context.ClaveCampana,
                validRequest.IdSubCartera,
                validRequest.UnidadNegocio,
                cancellationToken));

        return CarteraOperacionResult<PromesasCarteraResponse>.Exito(
            PromesasCarteraResponseMapper.Map(context, row));

    }
}
