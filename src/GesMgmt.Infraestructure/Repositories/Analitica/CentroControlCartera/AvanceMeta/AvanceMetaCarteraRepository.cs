using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class AvanceMetaCarteraRepository(
    AnaliticaDbContext context)
    : IAvanceMetaCarteraRepository
{
    public Task<AvanceMetaCarteraContexto?> ResolverContextoAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        bool includeClientLevelTarget,
        CancellationToken cancellationToken) =>
        AvanceMetaCarteraEfConsulta.ResolverContextoAsync(
            context,
            idClienteCrm,
            codigoCampana,
            idSubCartera,
            unidadNegocio,
            includeClientLevelTarget,
            cancellationToken);

    public Task<AvanceMetaCarteraDbFila?> ObtenerAvanceMetaAsync(
        int claveCliente,
        int claveCampana,
        string? unidadNegocio,
        DateOnly fechaHasta,
        CancellationToken cancellationToken) =>
        AvanceMetaCarteraEfConsulta.ObtenerAsync(
            context,
            claveCliente,
            claveCampana,
            unidadNegocio,
            fechaHasta,
            cancellationToken);
}
