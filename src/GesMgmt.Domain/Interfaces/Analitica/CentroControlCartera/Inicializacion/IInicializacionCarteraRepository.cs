using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;

public interface IInicializacionCarteraRepository
{
    Task<InicializacionCarteraOrigen?> ResolverAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken);
}
