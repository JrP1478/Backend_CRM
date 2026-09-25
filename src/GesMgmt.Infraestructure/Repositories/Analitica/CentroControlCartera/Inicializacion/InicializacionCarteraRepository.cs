using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class InicializacionCarteraRepository(
    AnaliticaDbContext context)
    : IInicializacionCarteraRepository
{
    public async Task<InicializacionCarteraOrigen?> ResolverAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken)
    {
        var filterOptions = await OpcionesFiltroCarteraEfConsulta.EjecutarAsync(
            context,
            idClienteCrm,
            cancellationToken);

        if (filterOptions is null)
        {
            return null;
        }

        var overviewContext = await PanoramaCarteraContextoEfConsulta.EjecutarAsync(
            context,
            idClienteCrm,
            codigoCampana,
            idSubCartera,
            unidadNegocio,
            cancellationToken);

        return new InicializacionCarteraOrigen(
            filterOptions,
            overviewContext);
    }
}
