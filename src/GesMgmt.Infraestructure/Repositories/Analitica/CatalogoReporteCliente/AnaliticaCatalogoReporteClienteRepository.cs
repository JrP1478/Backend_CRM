using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaCatalogoReporteClienteRepository(
    AnaliticaDbContext context,
    ICacheAccesoAnalitica cache)
    : IAnaliticaCatalogoReporteClienteRepository
{
    public bool Soporta(int idOpcion) =>
        idOpcion == AnaliticaOpcionIds.GestionIntegralCobranza;

    public Task<IReadOnlyList<AnaliticaCatalogoReporteClienteItem>> ObtenerActualAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (!Soporta(idOpcion))
        {
            return Task.FromResult<IReadOnlyList<AnaliticaCatalogoReporteClienteItem>>(
                Array.Empty<AnaliticaCatalogoReporteClienteItem>());
        }

        return cache.GetOrCreateAsync<IReadOnlyList<AnaliticaCatalogoReporteClienteItem>>(
            ClavesCacheAccesoAnalitica.CatalogoClienteReporte(idOpcion),
            PoliticaCacheAccesoAnalitica.CatalogDuration,
            async token => await context.CatalogoReporteClienteAnalitica
                .AsNoTracking()
                .Where(catalog => catalog.IdOpcion == idOpcion)
                .OrderBy(catalog => catalog.ClienteReporte)
                .ThenBy(catalog => catalog.IdClienteCrm)
                .Select(catalog => new AnaliticaCatalogoReporteClienteItem
                {
                    IdClienteCrm = catalog.IdClienteCrm,
                    ClienteReporte = catalog.ClienteReporte
                })
                .ToArrayAsync(token),
            cancellationToken);
    }
}
