using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface ISeguimientoPromesasCarteraRepository
{
    Task<SeguimientoPromesasCarteraConsultaResult> ObtenerAsync(
        int idClienteCrm,
        int claveCliente,
        int claveCampana,
        long? idSubCartera,
        string? unidadNegocio,
        DateOnly fechaVencimiento,
        int pagina,
        int tamanoPagina,
        string? estado,
        string ordenarPor,
        string direccionOrden,
        CancellationToken cancellationToken);
}
