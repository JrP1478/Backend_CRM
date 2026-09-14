using System.Data;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaPublicacionReporteClienteWriter(
    AnaliticaDbContext context,
    ICacheAccesoAnalitica cache)
    : IAnaliticaPublicacionReporteClienteWriter
{
    public async Task PatchAsync(
        int idOpcion,
        IReadOnlyCollection<AnaliticaPublicacionReporteClienteActualizar> publicaciones,
        int actualizadoPor,
        CancellationToken cancellationToken)
    {
        if (publicaciones.Count == 0)
        {
            return;
        }

        var normalizedPublications = publicaciones
            .Select(publication => new PublicacionNormalizada(
                publication.IdCliente,
                publication.Nombre.Trim(),
                publication.IdsGrupos is null
                    ? null
                    : publication.IdsGrupos
                        .Where(idGrupo => idGrupo > 0)
                        .Distinct()
                        .OrderBy(idGrupo => idGrupo)
                        .ToArray(),
                string.IsNullOrWhiteSpace(publication.UrlIncrustacion)
                    ? null
                    : publication.UrlIncrustacion.Trim()))
            .ToArray();

        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var now = DateTime.UtcNow;

        foreach (var publication in normalizedPublications)
        {
            if (publication.IdsGrupos is not null)
            {
                await AplicarAlcancesGrupoAsync(
                    idOpcion,
                    publication,
                    actualizadoPor,
                    now,
                    cancellationToken);
            }

            await AplicarIncrustacionAsync(
                idOpcion,
                publication,
                actualizadoPor,
                now,
                cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        cache.Remove(ClavesCacheAccesoAnalitica.PublicacionesClienteReporte(idOpcion));
    }

    private async Task AplicarAlcancesGrupoAsync(
        int idOpcion,
        PublicacionNormalizada publication,
        int actualizadoPor,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var existingScopes = await context.AlcancesReporteClienteAnalitica
            .Where(scope =>
                scope.IdOpcion == idOpcion &&
                scope.IdClienteCrm == publication.IdCliente &&
                scope.ClienteReporte == publication.Nombre)
            .ToListAsync(cancellationToken);

        foreach (var scope in existingScopes.Where(scope => scope.EsActivo))
        {
            scope.EsActivo = false;
            scope.ActualizadoPor = actualizadoPor;
            scope.FechaActualizacion = now;
        }

        foreach (var idGrupo in publication.IdsGrupos!)
        {
            var scope = existingScopes.FirstOrDefault(
                item => item.IdGrupoSisges == idGrupo);

            if (scope is null)
            {
                await context.AlcancesReporteClienteAnalitica.AddAsync(
                    new AnaliticaAlcanceReporteClienteEntrada
                    {
                        IdOpcion = idOpcion,
                        IdClienteCrm = publication.IdCliente,
                        ClienteReporte = publication.Nombre,
                        IdGrupoSisges = idGrupo,
                        EsActivo = true,
                        CreadoPor = actualizadoPor,
                        FechaCreacion = now,
                        ActualizadoPor = actualizadoPor,
                        FechaActualizacion = now
                    },
                    cancellationToken);
                continue;
            }

            scope.EsActivo = true;
            scope.ActualizadoPor = actualizadoPor;
            scope.FechaActualizacion = now;
        }
    }

    private async Task AplicarIncrustacionAsync(
        int idOpcion,
        PublicacionNormalizada publication,
        int actualizadoPor,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var embed = await context.IncrustacionesReporteClienteAnalitica
            .SingleOrDefaultAsync(
                item =>
                    item.IdOpcion == idOpcion &&
                    item.IdClienteCrm == publication.IdCliente &&
                    item.ClienteReporte == publication.Nombre,
                cancellationToken);

        if (publication.UrlIncrustacion is null)
        {
            if (embed is not null && embed.EsActivo)
            {
                embed.EsActivo = false;
                embed.ActualizadoPor = actualizadoPor;
                embed.FechaActualizacion = now;
            }

            return;
        }

        if (embed is null)
        {
            await context.IncrustacionesReporteClienteAnalitica.AddAsync(
                new AnaliticaIncrustacionReporteClienteEntrada
                {
                    IdOpcion = idOpcion,
                    IdClienteCrm = publication.IdCliente,
                    ClienteReporte = publication.Nombre,
                    UrlIncrustacion = publication.UrlIncrustacion,
                    EsActivo = true,
                    CreadoPor = actualizadoPor,
                    FechaCreacion = now,
                    ActualizadoPor = actualizadoPor,
                    FechaActualizacion = now
                },
                cancellationToken);
            return;
        }

        embed.UrlIncrustacion = publication.UrlIncrustacion;
        embed.EsActivo = true;
        embed.ActualizadoPor = actualizadoPor;
        embed.FechaActualizacion = now;
    }

    private sealed record PublicacionNormalizada(
        int IdCliente,
        string Nombre,
        IReadOnlyCollection<int>? IdsGrupos,
        string? UrlIncrustacion);
}
