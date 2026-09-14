using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class PromesasVencidasCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IPromesasCarteraRepository promisesRepository,
    IPromesasVencidasCarteraRepository repository) : IPromesasVencidasCarteraService
{
    public async Task<CarteraOperacionResult<PromesasVencidasCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? pagina,
        string? tamanoPagina,
        string? antiguedad,
        string? ordenarPor,
        string? direccionOrden,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!PromesasVencidasCarteraRequest.IntentarCrear(
                campana,
                idSubCartera,
                pagina,
                tamanoPagina,
                antiguedad,
                ordenarPor,
                direccionOrden,
                out var request,
                out var requestErrors))
        {
            return CarteraOperacionResult<PromesasVencidasCarteraResponse>.Validacion(requestErrors);
        }

        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var normalizedBusinessUnit,
                out var businessUnitErrors))
        {
            return CarteraOperacionResult<PromesasVencidasCarteraResponse>.Validacion(businessUnitErrors);
        }

        var validRequest = request!;

        var clientAccess = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!clientAccess.EstaPermitido)
        {
            return CarteraOperacionResult<PromesasVencidasCarteraResponse>.DesdeAcceso(clientAccess);
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
            () => promisesRepository.ResolverContextoAsync(
                effectiveCrmClientId,
                validRequest.Campana,
                validRequest.IdSubCartera,
                validRequest.UnidadNegocio,
                cancellationToken));

        if (context is null)
        {
            return CarteraOperacionResult<PromesasVencidasCarteraResponse>.Problema(
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

        var result = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseConsulta,
            () => repository.ObtenerAsync(
                context.ClaveCliente,
                context.ClaveCampana,
                validRequest.IdSubCartera,
                validRequest.UnidadNegocio,
                validRequest.Pagina,
                validRequest.TamanoPagina,
                validRequest.Antiguedad,
                validRequest.OrdenarPor,
                validRequest.DireccionOrden,
                cancellationToken));

        return CarteraOperacionResult<PromesasVencidasCarteraResponse>.Exito(
            PromesasVencidasCarteraResponseMapper.Map(
                context,
                result));

    }
}
