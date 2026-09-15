using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IAvanceMetaCarteraRepository
{
    Task<AvanceMetaCarteraContexto?> ResolverContextoAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        bool includeClientLevelTarget,
        CancellationToken cancellationToken);

    Task<AvanceMetaCarteraDbFila?> ObtenerAvanceMetaAsync(
        int claveCliente,
        int claveCampana,
        string? unidadNegocio,
        DateOnly fechaHasta,
        CancellationToken cancellationToken);
}
