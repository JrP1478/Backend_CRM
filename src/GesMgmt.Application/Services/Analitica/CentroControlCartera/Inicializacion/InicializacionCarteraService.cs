using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class InicializacionCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IInicializacionCarteraRepository bootstrapRepository,
    IPanoramaCarteraRepository overviewRepository) : IInicializacionCarteraService
{
    public async Task<CarteraOperacionResult<InicializacionCarteraResponse>> ObtenerAsync(
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
            return CarteraOperacionResult<InicializacionCarteraResponse>.Validacion(requestErrors);
        }

        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var normalizedBusinessUnit,
                out var businessUnitErrors))
        {
            return CarteraOperacionResult<InicializacionCarteraResponse>.Validacion(businessUnitErrors);
        }

        var validRequest = request!;
        var clientAccess = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!clientAccess.EstaPermitido)
        {
            return CarteraOperacionResult<InicializacionCarteraResponse>.DesdeAcceso(clientAccess);
        }

        var effectiveCrmClientId = clientAccess.IdClienteCrm!.Value;
        validRequest = validRequest with
        {
            UnidadNegocio = UnidadNegocioCarteraPolicy.ResolverSolicitadoOPredeterminado(
                effectiveCrmClientId,
                normalizedBusinessUnit)
        };
        var source = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseContexto,
            () => bootstrapRepository.ResolverAsync(
                effectiveCrmClientId,
                validRequest.Campana,
                validRequest.IdSubCartera,
                validRequest.UnidadNegocio,
                cancellationToken));

        if (source is null)
        {
            return CarteraOperacionResult<InicializacionCarteraResponse>.Problema(
                codigoEstado: 404,
                titulo: "Cliente Analítica no encontrado",
                detalle: "No existe un cliente Analítica para el scope autorizado.");
        }

        if (!UnidadNegocioCarteraContrato.IntentarResolver(
                normalizedBusinessUnit,
                UnidadNegocioCarteraPolicy.ObtenerPredeterminadoCompatible(
                    effectiveCrmClientId),
                source.FilterOptions.UnidadesNegocio,
                out var businessUnitSelection,
                out var selectionErrors))
        {
            return CarteraOperacionResult<InicializacionCarteraResponse>.Validacion(selectionErrors);
        }

        validRequest = validRequest with
        {
            UnidadNegocio = businessUnitSelection.SelectedBusinessUnit
        };

        var filterOptions = OpcionesFiltroCarteraResponseMapper.Map(
            source.FilterOptions,
            effectiveCrmClientId,
            validRequest.UnidadNegocio);

        var context = source.PanoramaContext;
        if (context is null)
        {
            if (validRequest.Campana is not null
                || validRequest.IdSubCartera is not null)
            {
                return CampanaNoEncontrada(validRequest);
            }

            return CarteraOperacionResult<InicializacionCarteraResponse>.Exito(
                new InicializacionCarteraResponse(
                    filterOptions,
                    Panorama: null));
        }

        if (validRequest.IdSubCartera is not null
            && !context.SubCarteraOperativaDisponible)
        {
            return CarteraOperacionResult<InicializacionCarteraResponse>.Problema(
                codigoEstado: 404,
                titulo: "Campaña Analítica no encontrada",
                detalle: $"No existe la campaña '{context.Resumen.CodigoCampana}' asociada a la subcartera Analítica '{validRequest.IdSubCartera}' dentro del cliente autorizado.");
        }

        var summaryContext = context.Resumen;
        if (!validRequest.IntentarResolverRango(
                summaryContext,
                out var range,
                out var rangeErrors))
        {
            return CarteraOperacionResult<InicializacionCarteraResponse>.Validacion(rangeErrors);
        }

        var rows = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseConsulta,
            () => overviewRepository.ObtenerPanoramaAsync(
                summaryContext.ClaveCliente,
                summaryContext.ClaveCampana,
                validRequest.IdSubCartera,
                validRequest.UnidadNegocio,
                UnidadNegocioCarteraPolicy.PuedeUsarMetaNivelCliente(
                    effectiveCrmClientId,
                    validRequest.UnidadNegocio),
                range,
                cancellationToken));

        if (!rows.Resumen.FechaCorte.HasValue)
        {
            return CarteraOperacionResult<InicializacionCarteraResponse>.Problema(
                codigoEstado: 422,
                titulo: "Corte de cartera no disponible",
                detalle:
                    "No existe un snapshot real de cartera dentro del rango solicitado. " +
                    "El resumen no fabricará assigned/managed/pending desde filas carry-forward.");
        }

        return CarteraOperacionResult<InicializacionCarteraResponse>.Exito(
            new InicializacionCarteraResponse(
                filterOptions,
                PanoramaCarteraResponseMapper.Map(
                    summaryContext,
                    range,
                    rows)));

    }


    private static CarteraOperacionResult<InicializacionCarteraResponse> CampanaNoEncontrada(
        ResumenCarteraRequest request) =>
        CarteraOperacionResult<InicializacionCarteraResponse>.Problema(
            codigoEstado: 404,
            titulo: "Campaña Analítica no encontrada",
            detalle: request.IdSubCartera is not null
                ? request.Campana is null
                    ? $"No existe una campaña disponible para la subcartera Analítica '{request.IdSubCartera}' dentro del cliente autorizado."
                    : $"No existe la campaña '{request.Campana}' asociada a la subcartera Analítica '{request.IdSubCartera}' dentro del cliente autorizado."
                : request.Campana is null
                    ? "No existe una campaña disponible para el cliente autorizado."
                    : $"No existe la campaña '{request.Campana}' para el cliente autorizado.");
}
