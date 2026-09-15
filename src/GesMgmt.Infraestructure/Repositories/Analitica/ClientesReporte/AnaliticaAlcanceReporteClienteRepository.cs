using System.Data;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaAlcanceReporteClienteRepository(
    AnaliticaDbContext context)
    : IAnaliticaAlcanceReporteClienteRepository
{
    public Task<bool> TieneAlgunAlcanceAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        context.AlcancesReporteClienteAnalitica
            .AsNoTracking()
            .AnyAsync(
                scope =>
                    scope.IdOpcion == idOpcion &&
                    scope.EsActivo,
                cancellationToken);

    public async Task<IReadOnlyList<AnaliticaAlcanceReporteClienteMapeo>> ObtenerMapeosAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        await context.AlcancesReporteClienteAnalitica
            .AsNoTracking()
            .Where(scope =>
                scope.IdOpcion == idOpcion &&
                scope.EsActivo)
            .OrderBy(scope => scope.ClienteReporte)
            .ThenBy(scope => scope.IdClienteCrm)
            .ThenBy(scope => scope.IdGrupoSisges)
            .Select(scope => new AnaliticaAlcanceReporteClienteMapeo
            {
                IdClienteCrm = scope.IdClienteCrm,
                ClienteReporte = scope.ClienteReporte,
                IdGrupoSisges = scope.IdGrupoSisges
            })
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyList<int>> ObtenerIdsOpcionesConAlcanceActivoAsync(
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
            return Array.Empty<int>();
        }

        return await context.AlcancesReporteClienteAnalitica
            .AsNoTracking()
            .Where(scope =>
                scope.EsActivo &&
                normalizedOptionIds.Contains(scope.IdOpcion))
            .Select(scope => scope.IdOpcion)
            .Distinct()
            .OrderBy(idOpcion => idOpcion)
            .ToArrayAsync(cancellationToken);
    }

    public async Task ReemplazarParaClienteAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        IReadOnlyCollection<int> idsGrupos,
        int actualizadoPor,
        CancellationToken cancellationToken)
    {
        var normalizedName = clienteReporte.Trim();
        var normalizedGroupIds = idsGrupos
            .Where(idGrupo => idGrupo > 0)
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToArray();

        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var existingScopes = await context.AlcancesReporteClienteAnalitica
            .Where(scope =>
                scope.IdOpcion == idOpcion &&
                scope.IdClienteCrm == idClienteCrm &&
                scope.ClienteReporte == normalizedName)
            .ToListAsync(cancellationToken);

        var now = DateTime.UtcNow;

        foreach (var scope in existingScopes.Where(scope => scope.EsActivo))
        {
            scope.EsActivo = false;
            scope.ActualizadoPor = actualizadoPor;
            scope.FechaActualizacion = now;
        }

        foreach (var idGrupo in normalizedGroupIds)
        {
            var scope = existingScopes.FirstOrDefault(
                item => item.IdGrupoSisges == idGrupo);

            if (scope is null)
            {
                await context.AlcancesReporteClienteAnalitica.AddAsync(
                    new AnaliticaAlcanceReporteClienteEntrada
                    {
                        IdOpcion = idOpcion,
                        IdClienteCrm = idClienteCrm,
                        ClienteReporte = normalizedName,
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

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
