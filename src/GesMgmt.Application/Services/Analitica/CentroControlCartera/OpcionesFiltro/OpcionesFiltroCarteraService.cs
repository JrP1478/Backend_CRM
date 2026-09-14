using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class OpcionesFiltroCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IOpcionesFiltroCarteraRepository repository) : IOpcionesFiltroCarteraService
{
    public async Task<CarteraOperacionResult<OpcionesFiltroCarteraResponse>> ObtenerAsync(
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var normalizedBusinessUnit,
                out var businessUnitErrors))
        {
            return CarteraOperacionResult<OpcionesFiltroCarteraResponse>.Validacion(businessUnitErrors);
        }

        var clientAccess = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!clientAccess.EstaPermitido)
        {
            return CarteraOperacionResult<OpcionesFiltroCarteraResponse>.DesdeAcceso(clientAccess);
        }

        var effectiveCrmClientId = clientAccess.IdClienteCrm!.Value;
        var source = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseConsulta,
            () => repository.ObtenerOpcionesFiltroAsync(
                effectiveCrmClientId,
                cancellationToken));

        if (source is null)
        {
            return CarteraOperacionResult<OpcionesFiltroCarteraResponse>.Problema(
                codigoEstado: 404,
                titulo: "Cliente Analítica no encontrado",
                detalle: "No existe un cliente Analítica para el scope autorizado.");
        }

        if (!UnidadNegocioCarteraContrato.IntentarResolver(
                normalizedBusinessUnit,
                UnidadNegocioCarteraPolicy.ObtenerPredeterminadoCompatible(
                    effectiveCrmClientId),
                source.UnidadesNegocio,
                out var businessUnitSelection,
                out var selectionErrors))
        {
            return CarteraOperacionResult<OpcionesFiltroCarteraResponse>.Validacion(selectionErrors);
        }

        return CarteraOperacionResult<OpcionesFiltroCarteraResponse>.Exito(
            OpcionesFiltroCarteraResponseMapper.Map(
                source,
                effectiveCrmClientId,
                businessUnitSelection.SelectedBusinessUnit));

    }
}
