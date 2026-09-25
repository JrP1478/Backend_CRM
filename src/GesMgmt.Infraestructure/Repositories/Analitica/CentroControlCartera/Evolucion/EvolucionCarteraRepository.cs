using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class EvolucionCarteraRepository(AnaliticaDbContext context)
    : IEvolucionCarteraRepository
{
    public Task<EvolucionCarteraContexto?> ResolverContextoAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken) =>
        EvolucionCarteraEfConsulta.ResolverContextoAsync(
            context,
            idClienteCrm,
            codigoCampana,
            idSubCartera,
            unidadNegocio,
            cancellationToken);

    public Task<IReadOnlyList<EvolucionCarteraDbFila>> ObtenerEvolucionAsync(
        int claveCliente,
        int claveCampana,
        long? idSubCartera,
        string? unidadNegocio,
        RangoEvolucionCartera range,
        CancellationToken cancellationToken) =>
        EvolucionCarteraEfConsulta.ObtenerAsync(
            context,
            claveCliente,
            claveCampana,
            idSubCartera,
            unidadNegocio,
            range,
            cancellationToken);
}
