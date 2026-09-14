using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IRendimientoAsesorCarteraRepository
{
    Task<IReadOnlyList<RendimientoAsesorCarteraDbFila>?> ObtenerRendimientoAsesorAsync(
        int idClienteCrm,
        RendimientoAsesorCarteraRequest request,
        CancellationToken cancellationToken);
}
