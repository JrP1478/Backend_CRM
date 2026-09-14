using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface IPromesasVencidasCarteraService
{
    Task<CarteraOperacionResult<PromesasVencidasCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? pagina,
        string? tamanoPagina,
        string? antiguedad,
        string? ordenarPor,
        string? direccionOrden,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken);
}
