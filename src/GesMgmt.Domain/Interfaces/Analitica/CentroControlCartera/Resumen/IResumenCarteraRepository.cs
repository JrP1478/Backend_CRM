using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IResumenCarteraRepository
{
    Task<ResumenCarteraContexto?> ResolverContextoAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken);

    Task<ResumenCarteraDbFila> ObtenerResumenAsync(
        int claveCliente,
        int claveCampana,
        long? idSubCartera,
        string? unidadNegocio,
        RangoResumenCartera range,
        CancellationToken cancellationToken);
}
