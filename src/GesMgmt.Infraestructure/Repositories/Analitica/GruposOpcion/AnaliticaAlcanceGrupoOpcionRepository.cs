using System.Data;
using System.Text.Json;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaAlcanceGrupoOpcionRepository(
    GesMgmt.Infraestructure.Persistence.AnaliticaDbContext context)
    : IAnaliticaAlcanceGrupoOpcionRepository
{
    public Task<bool> TieneAlgunAlcanceAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        context.AlcancesOpcionGrupoAnalitica
            .AsNoTracking()
            .AnyAsync(
                scope => scope.IdOpcion == idOpcion,
                cancellationToken);

    public async Task<IReadOnlyList<int>> ObtenerIdsGruposAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        await context.AlcancesOpcionGrupoAnalitica
            .AsNoTracking()
            .Where(scope =>
                scope.IdOpcion == idOpcion &&
                scope.EsActivo)
            .OrderBy(scope => scope.IdGrupoSisges)
            .Select(scope => scope.IdGrupoSisges)
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyList<AlcanceOpcionGrupoAnalitica>> ObtenerAlcancesAsync(
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken)
    {
        var normalizedOptionIds = idsOpciones
            .Where(idOpcion => idOpcion > 0)
            .Distinct()
            .OrderBy(idOpcion => idOpcion)
            .ToArray();

        if (normalizedOptionIds.Length == 0)
        {
            return Array.Empty<AlcanceOpcionGrupoAnalitica>();
        }

        return await context.AlcancesOpcionGrupoAnalitica
            .AsNoTracking()
            .Where(scope => normalizedOptionIds.Contains(scope.IdOpcion))
            .OrderBy(scope => scope.IdOpcion)
            .ThenBy(scope => scope.IdGrupoSisges)
            .Select(scope => new AlcanceOpcionGrupoAnalitica
            {
                IdOpcion = scope.IdOpcion,
                IdGrupoSisges = scope.IdGrupoSisges,
                EsActivo = scope.EsActivo
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task ReemplazarAsync(
        int idOpcion,
        IReadOnlyCollection<int> previousGroupIds,
        IReadOnlyCollection<int> idsGrupos,
        int? idUsuario,
        CancellationToken cancellationToken)
    {
        var normalizedPreviousGroupIds = Normalizar(previousGroupIds);
        var normalizedGroupIds = Normalizar(idsGrupos);
        var requestedGroupIds = normalizedGroupIds.ToHashSet();
        var now = DateTime.UtcNow;

        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var existingScopes = await context.AlcancesOpcionGrupoAnalitica
            .Where(scope => scope.IdOpcion == idOpcion)
            .ToListAsync(cancellationToken);

        foreach (var scope in existingScopes)
        {
            if (requestedGroupIds.Contains(scope.IdGrupoSisges))
            {
                scope.EsActivo = true;
                scope.ActualizadoPor = idUsuario;
                scope.FechaActualizacion = now;
                requestedGroupIds.Remove(scope.IdGrupoSisges);
                continue;
            }

            if (!scope.EsActivo)
            {
                continue;
            }

            scope.EsActivo = false;
            scope.ActualizadoPor = idUsuario;
            scope.FechaActualizacion = now;
        }

        foreach (var idGrupo in requestedGroupIds.OrderBy(idGrupo => idGrupo))
        {
            await context.AlcancesOpcionGrupoAnalitica.AddAsync(
                new AlcanceOpcionGrupoAnalitica
                {
                    IdOpcion = idOpcion,
                    IdGrupoSisges = idGrupo,
                    EsActivo = true,
                    CreadoPor = idUsuario,
                    FechaCreacion = now,
                    ActualizadoPor = idUsuario,
                    FechaActualizacion = now
                },
                cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        var previousGroupIdsJson = JsonSerializer.Serialize(normalizedPreviousGroupIds);
        var newGroupIdsJson = JsonSerializer.Serialize(normalizedGroupIds);

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
                {idUsuario},
                {now}
            );
            """,
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private static int[] Normalizar(IReadOnlyCollection<int> idsGrupos) =>
        idsGrupos
            .Where(idGrupo => idGrupo > 0)
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToArray();
}
