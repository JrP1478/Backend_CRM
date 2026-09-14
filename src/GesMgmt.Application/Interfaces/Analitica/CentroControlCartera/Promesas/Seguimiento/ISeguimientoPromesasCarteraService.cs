using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface ISeguimientoPromesasCarteraService
{
    Task<CarteraOperacionResult<SeguimientoPromesasCarteraResponse>> ObtenerAsync(
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
        CancellationToken cancellationToken);
}
