using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class RendimientoAsesorCarteraRepository(AnaliticaDbContext context)
    : IRendimientoAsesorCarteraRepository
{
    public Task<IReadOnlyList<RendimientoAsesorCarteraDbFila>?> ObtenerRendimientoAsesorAsync(
        int idClienteCrm,
        RendimientoAsesorCarteraRequest request,
        CancellationToken cancellationToken) =>
        RendimientoPersonasCarteraEfConsulta.ObtenerAsesoresAsync(
            context,
            idClienteCrm,
            request,
            cancellationToken);
}
