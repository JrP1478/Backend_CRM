using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface IOpcionesFiltroCarteraService
{
    Task<CarteraOperacionResult<OpcionesFiltroCarteraResponse>> ObtenerAsync(
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken);
}
