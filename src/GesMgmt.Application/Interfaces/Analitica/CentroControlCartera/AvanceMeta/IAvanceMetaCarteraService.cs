using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface IAvanceMetaCarteraService
{
    Task<CarteraOperacionResult<AvanceMetaCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? fechaHasta,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken);
}
