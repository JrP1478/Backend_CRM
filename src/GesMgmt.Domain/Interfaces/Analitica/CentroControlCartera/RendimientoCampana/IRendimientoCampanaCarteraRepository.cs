using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IRendimientoCampanaCarteraRepository
{
    Task<IReadOnlyList<RendimientoCampanaCarteraDbFila>?> ObtenerRendimientoCampanaAsync(
        int idClienteCrm,
        RendimientoCampanaCarteraRequest request,
        CancellationToken cancellationToken);
}
