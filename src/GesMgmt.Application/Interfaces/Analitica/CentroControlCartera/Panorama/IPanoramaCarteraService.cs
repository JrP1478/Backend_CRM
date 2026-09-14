using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface IPanoramaCarteraService
{
    Task<CarteraOperacionResult<PanoramaCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? fechaDesde,
        string? fechaHasta,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken);
}
