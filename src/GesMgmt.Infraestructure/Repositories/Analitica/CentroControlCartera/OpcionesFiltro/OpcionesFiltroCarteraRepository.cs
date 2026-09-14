using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class OpcionesFiltroCarteraRepository(
    AnaliticaDbContext context)
    : IOpcionesFiltroCarteraRepository
{
    public Task<OpcionesFiltroCarteraDbResult?> ObtenerOpcionesFiltroAsync(
        int idClienteCrm,
        CancellationToken cancellationToken) =>
        OpcionesFiltroCarteraEfConsulta.EjecutarAsync(
            context,
            idClienteCrm,
            cancellationToken);
}
