using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IPanoramaCarteraRepository
{
    Task<PanoramaCarteraContexto?> ResolverContextoAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken);

    Task<PanoramaCarteraDbFilas> ObtenerPanoramaAsync(
        int claveCliente,
        int claveCampana,
        long? idSubCartera,
        string? unidadNegocio,
        bool includeClientLevelTarget,
        RangoResumenCartera range,
        CancellationToken cancellationToken);
}
