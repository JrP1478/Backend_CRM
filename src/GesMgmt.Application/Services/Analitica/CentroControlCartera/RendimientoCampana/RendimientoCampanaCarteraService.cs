using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class RendimientoCampanaCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IRendimientoCampanaCarteraRepository repository) : IRendimientoCampanaCarteraService
{
    public async Task<CarteraOperacionResult<RendimientoCampanaCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? idSupervisor,
        string? fechaDesde,
        string? fechaHasta,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!RendimientoCampanaCarteraRequest.IntentarCrear(
                campana,
                idSubCartera,
                idSupervisor,
                fechaDesde,
                fechaHasta,
                out var request,
                out var requestErrors))
        {
            return CarteraOperacionResult<RendimientoCampanaCarteraResponse>.Validacion(requestErrors);
        }

        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var normalizedBusinessUnit,
                out var businessUnitErrors))
        {
            return CarteraOperacionResult<RendimientoCampanaCarteraResponse>.Validacion(businessUnitErrors);
        }

        var clientAccess = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!clientAccess.EstaPermitido)
        {
            return CarteraOperacionResult<RendimientoCampanaCarteraResponse>.DesdeAcceso(clientAccess);
        }

        var effectiveCrmClientId = clientAccess.IdClienteCrm!.Value;
        var validRequest = request! with
        {
            UnidadNegocio = UnidadNegocioCarteraPolicy.ResolverSolicitadoOPredeterminado(
                effectiveCrmClientId,
                normalizedBusinessUnit)
        };
        var rows = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseConsulta,
            () => repository.ObtenerRendimientoCampanaAsync(
                effectiveCrmClientId,
                validRequest,
                cancellationToken));

        if (rows is null)
        {
            return CarteraOperacionResult<RendimientoCampanaCarteraResponse>.Problema(
                codigoEstado: 404,
                titulo: "Cliente Analítica no encontrado",
                detalle: "No existe un cliente Analítica para el scope autorizado.");
        }

        return CarteraOperacionResult<RendimientoCampanaCarteraResponse>.Exito(
            RendimientoCampanaCarteraResponseMapper.Map(rows));

    }
}
