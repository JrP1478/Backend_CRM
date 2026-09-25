using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IPromesasVencenHoyCarteraRepository
{
    Task<PromesasVencenHoyCarteraConsultaResult> ObtenerAsync(
        int claveCliente,
        int claveCampana,
        long? idSubCartera,
        string? unidadNegocio,
        int pagina,
        int tamanoPagina,
        string? estado,
        string ordenarPor,
        string direccionOrden,
        CancellationToken cancellationToken);
}
