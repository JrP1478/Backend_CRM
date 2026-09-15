using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IPromesasVencidasCarteraRepository
{
    Task<PromesasVencidasCarteraConsultaResult> ObtenerAsync(
        int claveCliente,
        int claveCampana,
        long? idSubCartera,
        string? unidadNegocio,
        int pagina,
        int tamanoPagina,
        string? antiguedad,
        string ordenarPor,
        string direccionOrden,
        CancellationToken cancellationToken);
}
