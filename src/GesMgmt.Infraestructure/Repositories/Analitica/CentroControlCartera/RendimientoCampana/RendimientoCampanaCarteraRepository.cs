using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class RendimientoCampanaCarteraRepository(
    AnaliticaDbContext context)
    : IRendimientoCampanaCarteraRepository
{
    public Task<IReadOnlyList<RendimientoCampanaCarteraDbFila>?> ObtenerRendimientoCampanaAsync(
        int idClienteCrm,
        RendimientoCampanaCarteraRequest request,
        CancellationToken cancellationToken) =>
        RendimientoCampanaCarteraEfConsulta.ObtenerAsync(
            context,
            idClienteCrm,
            request,
            cancellationToken);
}
