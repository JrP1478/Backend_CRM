using System.Data;
using System.Text.Json;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaOpcionUsuarioRepository(
    GesMgmt.Infraestructure.Persistence.AnaliticaDbContext context)
    : IAnaliticaOpcionUsuarioRepository
{
    public Task<bool> TieneAccesoAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken) =>
        context.AlcancesUsuarioOpcionAnalitica
            .AsNoTracking()
            .AnyAsync(
                scope =>
                    scope.IdUsuario == idUsuario &&
                    scope.IdOpcion == idOpcion &&
                    scope.EsActivo,
                cancellationToken);

    public async Task<IReadOnlyList<OpcionUsuarioAnalitica>> ObtenerOpcionesUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken) =>
        await (
            from scope in context.AlcancesUsuarioOpcionAnalitica.AsNoTracking()
            join option in context.ConfiguracionesOpcionAnalitica.AsNoTracking()
                on scope.IdOpcion equals option.IdOpcion
            where scope.IdUsuario == idUsuario &&
                  scope.EsActivo &&
                  option.EsActivo
            orderby option.IdOpcion
            select new OpcionUsuarioAnalitica(
                option.IdOpcion,
                option.CodigoOpcion,
                option.NombreOpcion))
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyList<int>> ObtenerIdsUsuariosAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        await context.AlcancesUsuarioOpcionAnalitica
            .AsNoTracking()
            .Where(scope =>
                scope.IdOpcion == idOpcion &&
                scope.EsActivo)
            .OrderBy(scope => scope.IdUsuario)
            .Select(scope => scope.IdUsuario)
            .ToArrayAsync(cancellationToken);

    public async Task ReemplazarAsync(
        int idOpcion,
        IReadOnlyCollection<int> previousUserIds,
        IReadOnlyCollection<int> idsUsuarios,
        int? adminUserId,
        CancellationToken cancellationToken)
    {
        var normalizedPreviousUserIds = Normalizar(previousUserIds);
        var normalizedUserIds = Normalizar(idsUsuarios);
        var requestedUserIds = normalizedUserIds.ToHashSet();
        var now = DateTime.UtcNow;

        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var existingScopes = await context.AlcancesUsuarioOpcionAnalitica
            .Where(scope => scope.IdOpcion == idOpcion)
            .ToListAsync(cancellationToken);

        foreach (var scope in existingScopes)
        {
            if (requestedUserIds.Contains(scope.IdUsuario))
            {
                scope.EsActivo = true;
                scope.ActualizadoPor = adminUserId;
                scope.FechaActualizacion = now;
                requestedUserIds.Remove(scope.IdUsuario);
                continue;
            }

            if (!scope.EsActivo)
            {
                continue;
            }

            scope.EsActivo = false;
            scope.ActualizadoPor = adminUserId;
            scope.FechaActualizacion = now;
        }

        foreach (var idUsuario in requestedUserIds.OrderBy(idUsuario => idUsuario))
        {
            await context.AlcancesUsuarioOpcionAnalitica.AddAsync(
                new AlcanceUsuarioOpcionAnalitica
                {
                    IdUsuario = idUsuario,
                    IdOpcion = idOpcion,
                    EsActivo = true,
                    CreadoPor = adminUserId,
                    FechaCreacion = now
                },
                cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        var previousUserIdsJson = JsonSerializer.Serialize(normalizedPreviousUserIds);
        var newUserIdsJson = JsonSerializer.Serialize(normalizedUserIds);

        await context.Database.ExecuteSqlInterpolatedAsync(
            $"""
            INSERT INTO acceso_analitica.auditoria_alcance_usuario_opcion
            (
                id_opcion,
                ids_usuarios_anteriores,
                ids_usuarios_nuevos,
                creado_por,
                fecha_creacion
            )
            VALUES
            (
                {idOpcion},
                {previousUserIdsJson},
                {newUserIdsJson},
                {adminUserId},
                {now}
            );
            """,
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private static int[] Normalizar(IReadOnlyCollection<int> idsUsuarios) =>
        idsUsuarios
            .Where(idUsuario => idUsuario > 0)
            .Distinct()
            .OrderBy(idUsuario => idUsuario)
            .ToArray();
}
