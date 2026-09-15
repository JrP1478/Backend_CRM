using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class RendimientoSupervisorCarteraRepository(AnaliticaDbContext context)
    : IRendimientoSupervisorCarteraRepository
{
    public Task<IReadOnlyList<RendimientoSupervisorCarteraDbFila>?> ObtenerRendimientoSupervisorAsync(
        int idClienteCrm,
        RendimientoSupervisorCarteraRequest request,
        CancellationToken cancellationToken) =>
        RendimientoPersonasCarteraEfConsulta.ObtenerSupervisoresAsync(
            context,
            idClienteCrm,
            request,
            cancellationToken);
}
