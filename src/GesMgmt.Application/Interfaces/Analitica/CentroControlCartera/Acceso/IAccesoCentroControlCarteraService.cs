using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface IAccesoCentroControlCarteraService
{
    Task<CentroControlCarteraClienteAcceso> ResolverClienteAsync(
        int? requestedCrmClientId,
        CancellationToken cancellationToken);
}
