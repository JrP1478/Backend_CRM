using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class AvanceMetaCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IAvanceMetaCarteraRepository repository) : IAvanceMetaCarteraService
{
    public async Task<CarteraOperacionResult<AvanceMetaCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? fechaHasta,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!AvanceMetaCarteraRequest.IntentarCrear(
                campana,
                idSubCartera,
                fechaHasta,
                out var request,
                out var requestErrors))
        {
            return CarteraOperacionResult<AvanceMetaCarteraResponse>.Validacion(requestErrors);
        }

        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var normalizedBusinessUnit,
                out var businessUnitErrors))
        {
            return CarteraOperacionResult<AvanceMetaCarteraResponse>.Validacion(businessUnitErrors);
        }

        var validRequest = request!;

        var clientAccess = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!clientAccess.EstaPermitido)
        {
            return CarteraOperacionResult<AvanceMetaCarteraResponse>.DesdeAcceso(clientAccess);
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
                UnidadNegocioCarteraPolicy.PuedeUsarMetaNivelCliente(
                    effectiveCrmClientId,
                    validRequest.UnidadNegocio),
                cancellationToken));

        if (context is null)
        {
            return CarteraOperacionResult<AvanceMetaCarteraResponse>.Problema(
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

        if (!validRequest.IntentarResolverFechaHasta(
                context,
                out var effectiveDateTo,
                out var dateErrors))
        {
            return CarteraOperacionResult<AvanceMetaCarteraResponse>.Validacion(dateErrors);
        }

        AvanceMetaCarteraDbFila? row = null;

        if (validRequest.IdSubCartera is null
            && UnidadNegocioCarteraPolicy.PuedeUsarMetaNivelCliente(
                effectiveCrmClientId,
                validRequest.UnidadNegocio))
        {
            row = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
                DiagnosticosCentroControlCartera.FaseConsulta,
                () => repository.ObtenerAvanceMetaAsync(
                    context.ClaveCliente,
                    context.ClaveCampana,
                    validRequest.UnidadNegocio,
                    effectiveDateTo,
                    cancellationToken));
        }

        return CarteraOperacionResult<AvanceMetaCarteraResponse>.Exito(
            AvanceMetaCarteraResponseMapper.Map(
                context,
                effectiveDateTo,
                row));

    }
}
