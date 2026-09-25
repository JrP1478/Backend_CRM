using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface IRendimientoAsesorCarteraService
{
    Task<CarteraOperacionResult<RendimientoAsesorCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? idSupervisor,
        string? fechaDesde,
        string? fechaHasta,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken);
}
