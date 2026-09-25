using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IOpcionesFiltroCarteraRepository
{
    Task<OpcionesFiltroCarteraDbResult?> ObtenerOpcionesFiltroAsync(
        int idClienteCrm,
        CancellationToken cancellationToken);
}
