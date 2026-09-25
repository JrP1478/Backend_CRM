using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class ResumenCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IResumenCarteraRepository repository) : IResumenCarteraService
{
    public async Task<CarteraOperacionResult<ResumenCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? fechaDesde,
        string? fechaHasta,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!ResumenCarteraRequest.IntentarCrear(
                campana,
                idSubCartera,
                fechaDesde,
                fechaHasta,
                out var request,
                out var requestErrors))
        {
            return CarteraOperacionResult<ResumenCarteraResponse>.Validacion(requestErrors);
        }

        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var normalizedBusinessUnit,
                out var businessUnitErrors))
        {
            return CarteraOperacionResult<ResumenCarteraResponse>.Validacion(businessUnitErrors);
        }

        var validRequest = request!;

        var clientAccess = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!clientAccess.EstaPermitido)
        {
            return CarteraOperacionResult<ResumenCarteraResponse>.DesdeAcceso(clientAccess);
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
            return CarteraOperacionResult<ResumenCarteraResponse>.Problema(
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

        if (!validRequest.IntentarResolverRango(
                context,
                out var range,
                out var rangeErrors))
        {
            return CarteraOperacionResult<ResumenCarteraResponse>.Validacion(rangeErrors);
        }

        var row = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseConsulta,
            () => repository.ObtenerResumenAsync(
                context.ClaveCliente,
                context.ClaveCampana,
                validRequest.IdSubCartera,
                validRequest.UnidadNegocio,
                range,
                cancellationToken));

        if (!row.FechaCorte.HasValue)
        {
            return CarteraOperacionResult<ResumenCarteraResponse>.Problema(
                codigoEstado: 422,
                titulo: "Corte de cartera no disponible",
                detalle:
                    "No existe un snapshot real de cartera dentro del rango solicitado. " +
                    "El resumen no fabricará assigned/managed/pending desde filas carry-forward.");
        }

        return CarteraOperacionResult<ResumenCarteraResponse>.Exito(
            ResumenCarteraResponseMapper.Map(
                context,
                range,
                row));

    }
}
