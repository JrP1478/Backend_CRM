using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Utils.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

public sealed class SeguimientoPromesasCarteraService(
    IAccesoCentroControlCarteraService accessService,
    IPromesasCarteraRepository promisesRepository,
    ISeguimientoPromesasCarteraRepository repository) : ISeguimientoPromesasCarteraService
{
    public async Task<CarteraOperacionResult<SeguimientoPromesasCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? fechaVencimiento,
        string? pagina,
        string? tamanoPagina,
        string? estado,
        string? ordenarPor,
        string? direccionOrden,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!SeguimientoPromesasCarteraRequest.IntentarCrear(
                campana,
                idSubCartera,
                fechaVencimiento,
                pagina,
                tamanoPagina,
                estado,
                ordenarPor,
                direccionOrden,
                out var request,
                out var erroresSolicitud))
        {
            return CarteraOperacionResult<SeguimientoPromesasCarteraResponse>.Validacion(erroresSolicitud);
        }

        if (!UnidadNegocioCarteraContrato.IntentarNormalizarSolicitado(
                unidadNegocio,
                out var unidadNegocioNormalizada,
                out var erroresUnidadNegocio))
        {
            return CarteraOperacionResult<SeguimientoPromesasCarteraResponse>.Validacion(erroresUnidadNegocio);
        }

        var solicitudValida = request!;

        var accesoCliente = await accessService.ResolverClienteAsync(
            idClienteCrm,
            cancellationToken);

        if (!accesoCliente.EstaPermitido)
        {
            return CarteraOperacionResult<SeguimientoPromesasCarteraResponse>.DesdeAcceso(accesoCliente);
        }

        var idClienteCrmEfectivo = accesoCliente.IdClienteCrm!.Value;
        solicitudValida = solicitudValida with
        {
            UnidadNegocio = UnidadNegocioCarteraPolicy.ResolverSolicitadoOPredeterminado(
                idClienteCrmEfectivo,
                unidadNegocioNormalizada)
        };

        var contexto = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseContexto,
            () => promisesRepository.ResolverContextoAsync(
                idClienteCrmEfectivo,
                solicitudValida.Campana,
                solicitudValida.IdSubCartera,
                solicitudValida.UnidadNegocio,
                cancellationToken));

        if (contexto is null)
        {
            return CarteraOperacionResult<SeguimientoPromesasCarteraResponse>.Problema(
                codigoEstado: 404,
                titulo: "Campaña Analítica no encontrada",
                detalle: solicitudValida.IdSubCartera is not null
                    ? solicitudValida.Campana is null
                        ? $"No existe una campaña disponible para la subcartera Analítica '{solicitudValida.IdSubCartera}' dentro del cliente autorizado."
                        : $"No existe la campaña '{solicitudValida.Campana}' asociada a la subcartera Analítica '{solicitudValida.IdSubCartera}' dentro del cliente autorizado."
                    : solicitudValida.Campana is null
                        ? "No existe una campaña disponible para el cliente autorizado."
                        : $"No existe la campaña '{solicitudValida.Campana}' para el cliente autorizado.");
        }

        var resultado = await DiagnosticosCentroControlCartera.ObservarFaseAsync(
            DiagnosticosCentroControlCartera.FaseConsulta,
            () => repository.ObtenerAsync(
                idClienteCrmEfectivo,
                contexto.ClaveCliente,
                contexto.ClaveCampana,
                solicitudValida.IdSubCartera,
                solicitudValida.UnidadNegocio,
                solicitudValida.FechaVencimiento,
                solicitudValida.Pagina,
                solicitudValida.TamanoPagina,
                solicitudValida.Estado,
                solicitudValida.OrdenarPor,
                solicitudValida.DireccionOrden,
                cancellationToken));

        return CarteraOperacionResult<SeguimientoPromesasCarteraResponse>.Exito(
            SeguimientoPromesasCarteraResponseMapper.Map(contexto, resultado));
    }
}
