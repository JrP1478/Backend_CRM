using System.Data;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaIncrustacionReporteClienteRepository(
    AnaliticaDbContext context,
    ICacheAccesoAnalitica cache)
    : IAnaliticaIncrustacionReporteClienteRepository
{
    public async Task<AnaliticaIncrustacionReporteClienteMapeo?> ObtenerAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        CancellationToken cancellationToken)
    {
        var normalizedName = clienteReporte.Trim();
        var publicaciones = await ObtenerActivasPorOpcionAsync(
            idOpcion,
            cancellationToken);

        var publication = publicaciones.FirstOrDefault(item =>
            item.IdClienteCrm == idClienteCrm &&
            string.Equals(
                item.ClienteReporte,
                normalizedName,
                StringComparison.OrdinalIgnoreCase));

        return publication is null
            ? null
            : new AnaliticaIncrustacionReporteClienteMapeo
            {
                ClienteReporte = publication.ClienteReporte,
                UrlIncrustacion = publication.UrlIncrustacion ?? string.Empty
            };
    }

    public Task<IReadOnlyList<AnaliticaIncrustacionReporteClienteAdministracionMapeo>> ObtenerActivasPorOpcionAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        cache.GetOrCreateAsync<IReadOnlyList<AnaliticaIncrustacionReporteClienteAdministracionMapeo>>(
            ClavesCacheAccesoAnalitica.PublicacionesClienteReporte(idOpcion),
            PoliticaCacheAccesoAnalitica.ConfigurationDuration,
            async token => await context.IncrustacionesReporteClienteAnalitica
                .AsNoTracking()
                .Where(embed =>
                    embed.IdOpcion == idOpcion &&
                    embed.EsActivo)
                .OrderBy(embed => embed.ClienteReporte)
                .ThenBy(embed => embed.IdClienteCrm)
                .Select(embed => new AnaliticaIncrustacionReporteClienteAdministracionMapeo
                {
                    IdClienteCrm = embed.IdClienteCrm,
                    ClienteReporte = embed.ClienteReporte,
                    UrlIncrustacion = embed.UrlIncrustacion
                })
                .ToArrayAsync(token),
            cancellationToken);

    public async Task GuardarAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        string urlIncrustacion,
        int actualizadoPor,
        CancellationToken cancellationToken)
    {
        var normalizedName = clienteReporte.Trim();
        var normalizedEmbedUrl = urlIncrustacion.Trim();

        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var entry = await context.IncrustacionesReporteClienteAnalitica
            .SingleOrDefaultAsync(
                embed =>
                    embed.IdOpcion == idOpcion &&
                    embed.IdClienteCrm == idClienteCrm &&
                    embed.ClienteReporte == normalizedName,
                cancellationToken);

        var now = DateTime.UtcNow;

        if (entry is null)
        {
            await context.IncrustacionesReporteClienteAnalitica.AddAsync(
                new AnaliticaIncrustacionReporteClienteEntrada
                {
                    IdOpcion = idOpcion,
                    IdClienteCrm = idClienteCrm,
                    ClienteReporte = normalizedName,
                    UrlIncrustacion = normalizedEmbedUrl,
                    EsActivo = true,
                    CreadoPor = actualizadoPor,
                    FechaCreacion = now,
                    ActualizadoPor = actualizadoPor,
                    FechaActualizacion = now
                },
                cancellationToken);
        }
        else
        {
            entry.UrlIncrustacion = normalizedEmbedUrl;
            entry.EsActivo = true;
            entry.ActualizadoPor = actualizadoPor;
            entry.FechaActualizacion = now;
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        cache.Remove(ClavesCacheAccesoAnalitica.PublicacionesClienteReporte(idOpcion));
    }

    public async Task DesactivarAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        int actualizadoPor,
        CancellationToken cancellationToken)
    {
        var normalizedName = clienteReporte.Trim();
        var entry = await context.IncrustacionesReporteClienteAnalitica
            .SingleOrDefaultAsync(
                embed =>
                    embed.IdOpcion == idOpcion &&
                    embed.IdClienteCrm == idClienteCrm &&
                    embed.ClienteReporte == normalizedName &&
                    embed.EsActivo,
                cancellationToken);

        if (entry is not null)
        {
            entry.EsActivo = false;
            entry.ActualizadoPor = actualizadoPor;
            entry.FechaActualizacion = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }

        cache.Remove(ClavesCacheAccesoAnalitica.PublicacionesClienteReporte(idOpcion));
    }
}
