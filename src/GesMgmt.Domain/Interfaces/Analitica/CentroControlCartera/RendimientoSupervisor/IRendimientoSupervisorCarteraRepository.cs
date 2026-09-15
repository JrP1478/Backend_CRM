using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IRendimientoSupervisorCarteraRepository
{
    Task<IReadOnlyList<RendimientoSupervisorCarteraDbFila>?> ObtenerRendimientoSupervisorAsync(
        int idClienteCrm,
        RendimientoSupervisorCarteraRequest request,
        CancellationToken cancellationToken);
}
