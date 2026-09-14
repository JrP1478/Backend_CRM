using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface IPromesasVencenHoyCarteraService
{
    Task<CarteraOperacionResult<PromesasVencenHoyCarteraResponse>> ObtenerAsync(
        string? campana,
        string? idSubCartera,
        string? pagina,
        string? tamanoPagina,
        string? estado,
        string? ordenarPor,
        string? direccionOrden,
        string? unidadNegocio,
        int? idClienteCrm,
        CancellationToken cancellationToken);
}
