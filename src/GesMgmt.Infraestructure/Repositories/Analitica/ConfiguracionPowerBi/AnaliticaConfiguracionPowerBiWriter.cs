using System.Data;
using System.Text.Json;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaConfiguracionPowerBiWriter(
    AnaliticaDbContext context,
    ICacheAccesoAnalitica cache)
    : IAnaliticaConfiguracionPowerBiWriter
{
    public async Task ActualizarAsync(
        int idOpcion,
        string codigoOpcion,
        string nombreOpcion,
        bool esActivo,
        IReadOnlyCollection<int> previousGroupIds,
        IReadOnlyCollection<int> idsGrupos,
        IReadOnlyCollection<AnaliticaPublicacionReporteClienteActualizar> publicaciones,
        int actualizadoPor,
        CancellationToken cancellationToken)
    {
        var normalizedPreviousGroupIds = NormalizarIdsGrupos(previousGroupIds);
        var normalizedGroupIds = NormalizarIdsGrupos(idsGrupos);
        var normalizedPublications = NormalizarPublicaciones(publicaciones);
        var groupsChanged = !normalizedPreviousGroupIds.SequenceEqual(normalizedGroupIds);
        var now = DateTime.UtcNow;

        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        await GuardarOpcionAsync(
            idOpcion,
            codigoOpcion.Trim(),
            nombreOpcion.Trim(),
            esActivo,
            actualizadoPor,
            now,
            cancellationToken);

        if (groupsChanged)
        {
            await ReemplazarGruposOpcionAsync(
                idOpcion,
                normalizedGroupIds,
                actualizadoPor,
                now,
                cancellationToken);
        }

        if (normalizedPublications.Length > 0)
        {
            await AplicarPublicacionesAsync(
                idOpcion,
                normalizedPublications,
                actualizadoPor,
                now,
                cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        if (groupsChanged)
        {
            await InsertarAuditoriaGrupoAsync(
                idOpcion,
                normalizedPreviousGroupIds,
                normalizedGroupIds,
                actualizadoPor,
                now,
                cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        cache.Remove(ClavesCacheAccesoAnalitica.ActiveOptions);

        if (normalizedPublications.Length > 0)
        {
            cache.Remove(ClavesCacheAccesoAnalitica.PublicacionesClienteReporte(idOpcion));
        }
    }

    private async Task GuardarOpcionAsync(
        int idOpcion,
        string codigoOpcion,
        string nombreOpcion,
        bool esActivo,
        int actualizadoPor,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var option = await context.ConfiguracionesOpcionAnalitica
            .SingleOrDefaultAsync(
                current => current.IdOpcion == idOpcion,
                cancellationToken);

        if (option is null)
        {
            await context.ConfiguracionesOpcionAnalitica.AddAsync(
                new ConfiguracionOpcionAnalitica
                {
                    IdOpcion = idOpcion,
                    CodigoOpcion = codigoOpcion,
                    NombreOpcion = nombreOpcion,
                    EsActivo = esActivo,
                    CreadoPor = actualizadoPor,
                    FechaCreacion = now
                },
                cancellationToken);
            return;
        }

        option.CodigoOpcion = codigoOpcion;
        option.NombreOpcion = nombreOpcion;
        option.EsActivo = esActivo;
        option.ActualizadoPor = actualizadoPor;
        option.FechaActualizacion = now;
    }

    private async Task ReemplazarGruposOpcionAsync(
        int idOpcion,
        IReadOnlyCollection<int> idsGrupos,
        int actualizadoPor,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var requestedGroupIds = idsGrupos.ToHashSet();
        var existingScopes = await context.AlcancesOpcionGrupoAnalitica
            .Where(scope => scope.IdOpcion == idOpcion)
            .ToListAsync(cancellationToken);

        foreach (var scope in existingScopes)
        {
            if (requestedGroupIds.Remove(scope.IdGrupoCrm))
            {
                scope.EsActivo = true;
                scope.ActualizadoPor = actualizadoPor;
                scope.FechaActualizacion = now;
                continue;
            }

            if (!scope.EsActivo)
            {
                continue;
            }

            scope.EsActivo = false;
            scope.ActualizadoPor = actualizadoPor;
            scope.FechaActualizacion = now;
        }

        foreach (var idGrupo in requestedGroupIds.OrderBy(idGrupo => idGrupo))
        {
            await context.AlcancesOpcionGrupoAnalitica.AddAsync(
                new AlcanceOpcionGrupoAnalitica
                {
                    IdOpcion = idOpcion,
                    IdGrupoCrm = idGrupo,
                    EsActivo = true,
                    CreadoPor = actualizadoPor,
                    FechaCreacion = now,
                    ActualizadoPor = actualizadoPor,
                    FechaActualizacion = now
                },
                cancellationToken);
        }
    }

    private async Task InsertarAuditoriaGrupoAsync(
        int idOpcion,
        IReadOnlyCollection<int> previousGroupIds,
        IReadOnlyCollection<int> idsGrupos,
        int actualizadoPor,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var previousGroupIdsJson = JsonSerializer.Serialize(previousGroupIds);
        var newGroupIdsJson = JsonSerializer.Serialize(idsGrupos);

        await context.Database.ExecuteSqlInterpolatedAsync(
            $"""
            INSERT INTO acceso_analitica.auditoria_alcance_opcion_grupo
            (
                id_opcion,
                ids_grupos_anteriores,
                ids_grupos_nuevos,
                creado_por,
                fecha_creacion
            )
            VALUES
            (
                {idOpcion},
                {previousGroupIdsJson},
                {newGroupIdsJson},
                {actualizadoPor},
                {now}
            );
            """,
            cancellationToken);
    }

    private async Task AplicarPublicacionesAsync(
        int idOpcion,
        IReadOnlyCollection<PublicacionNormalizada> publicaciones,
        int actualizadoPor,
        DateTime now,
        CancellationToken cancellationToken)
    {
        foreach (var publication in publicaciones)
        {
            if (publication.IdsGrupos is not null)
            {
                await AplicarGruposClienteReporteAsync(
                    idOpcion,
                    publication,
                    actualizadoPor,
                    now,
                    cancellationToken);
            }

            await AplicarIncrustacionClienteReporteAsync(
                idOpcion,
                publication,
                actualizadoPor,
                now,
                cancellationToken);
        }
    }

    private async Task AplicarGruposClienteReporteAsync(
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
                item => item.IdGrupoCrm == idGrupo);

            if (scope is null)
            {
                await context.AlcancesReporteClienteAnalitica.AddAsync(
                    new AnaliticaAlcanceReporteClienteEntrada
                    {
                        IdOpcion = idOpcion,
                        IdClienteCrm = publication.IdCliente,
                        ClienteReporte = publication.Nombre,
                        IdGrupoCrm = idGrupo,
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

    private async Task AplicarIncrustacionClienteReporteAsync(
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

    private static int[] NormalizarIdsGrupos(
        IEnumerable<int> idsGrupos) =>
        idsGrupos
            .Where(idGrupo => idGrupo > 0)
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToArray();

    private static PublicacionNormalizada[] NormalizarPublicaciones(
        IEnumerable<AnaliticaPublicacionReporteClienteActualizar> publicaciones) =>
        publicaciones
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

    private sealed record PublicacionNormalizada(
        int IdCliente,
        string Nombre,
        IReadOnlyCollection<int>? IdsGrupos,
        string? UrlIncrustacion);
}
