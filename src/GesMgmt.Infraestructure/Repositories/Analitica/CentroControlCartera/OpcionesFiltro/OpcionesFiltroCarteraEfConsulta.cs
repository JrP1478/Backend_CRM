using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal static class OpcionesFiltroCarteraEfConsulta
{
    public static async Task<OpcionesFiltroCarteraDbResult?> EjecutarAsync(
        AnaliticaDbContext context,
        int idClienteCrm,
        CancellationToken cancellationToken)
    {
        var claveCliente = await context.ClientesAnalitica
            .AsNoTracking()
            .Where(client => client.IdClienteCrm == idClienteCrm)
            .Select(client => (int?)client.ClaveCliente)
            .SingleOrDefaultAsync(cancellationToken);

        if (!claveCliente.HasValue)
        {
            return null;
        }

        var campanas = await CargarCampanasAsync(
            context,
            claveCliente.Value,
            cancellationToken);
        var subPortfolioContexts = await CargarContextosSubCarteraAsync(
            context,
            claveCliente.Value,
            cancellationToken);
        var supervisorContexts = await CargarContextosSupervisorAsync(
            context,
            claveCliente.Value,
            cancellationToken);

        return OpcionesFiltroCarteraDbResultMapper.Map(
            campanas,
            subPortfolioContexts,
            supervisorContexts);
    }

    private static async Task<IReadOnlyList<CarteraFiltroCampanaDbFila>> CargarCampanasAsync(
        AnaliticaDbContext context,
        int claveCliente,
        CancellationToken cancellationToken)
    {
        var summaryAvailability = await context.ResumenesDiariosCampanaAnalitica
            .AsNoTracking()
            .Where(row => row.ClaveCliente == claveCliente)
            .GroupBy(row => row.ClaveCampana)
            .Select(group => new DisponibilidadCampana(
                group.Key,
                group.Min(row => row.FechaCalendario),
                group.Max(row => row.FechaCalendario),
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var evolutionAvailability = await context.EvolucionDiariaCampanaAnalitica
            .AsNoTracking()
            .Where(row => row.ClaveCliente == claveCliente)
            .GroupBy(row => row.ClaveCampana)
            .Select(group => new DisponibilidadCampana(
                group.Key,
                group.Min(row => row.FechaCalendario),
                group.Max(row => row.FechaCalendario),
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var availabilityByCampaign = summaryAvailability
            .Concat(evolutionAvailability)
            .GroupBy(item => item.ClaveCampana)
            .ToDictionary(
                group => group.Key,
                group => new DisponibilidadCampana(
                    group.Key,
                    group.Min(item => item.FechaDisponibleDesde),
                    group.Max(item => item.FechaDisponibleHasta),
                    group.Max(item => item.FechaActualizacionUtc)));

        if (availabilityByCampaign.Count == 0)
        {
            return [];
        }

        var campaignKeys = availabilityByCampaign.Keys.ToArray();
        var campaignRows = await context.CampanasAnalitica
            .AsNoTracking()
            .Where(campana =>
                campana.ClaveCliente == claveCliente
                && campaignKeys.Contains(campana.ClaveCampana))
            .ToListAsync(cancellationToken);

        return campaignRows
            .Select(campana =>
            {
                var availability = availabilityByCampaign[campana.ClaveCampana];
                return new CarteraFiltroCampanaDbFila
                {
                    CodigoCampana = campana.CodigoCampana,
                    NombreCampana = campana.NombreCampana,
                    FechaInicio = campana.FechaInicio,
                    FechaFin = campana.FechaFin,
                    FechaDisponibleDesde = availability.FechaDisponibleDesde,
                    FechaDisponibleHasta = availability.FechaDisponibleHasta,
                    FechaActualizacionUtc = availability.FechaActualizacionUtc
                };
            })
            .OrderByDescending(row => row.FechaInicio)
            .ThenByDescending(row => row.CodigoCampana, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static async Task<IReadOnlyList<CarteraFiltroSubcarteraCampanaDbFila>> CargarContextosSubCarteraAsync(
        AnaliticaDbContext context,
        int claveCliente,
        CancellationToken cancellationToken)
    {
        var rawRows = await (
            from fact in context.HechosDiariosCarteraAnalitica.AsNoTracking()
            join portfolio in context.CarterasAnalitica.AsNoTracking()
                on fact.ClaveCartera equals portfolio.ClaveCartera
            join campana in context.CampanasAnalitica.AsNoTracking()
                on fact.ClaveCampana equals campana.ClaveCampana
            join date in context.FechasAnalitica.AsNoTracking()
                on fact.ClaveFecha equals date.ClaveFecha
            where fact.ClaveCliente == claveCliente
            group new { fact, portfolio, date } by new
            {
                fact.ClaveCartera,
                campana.CodigoCampana
            }
            into grouped
            select new ContextoSubCarteraCrudo(
                grouped.Key.ClaveCartera,
                grouped.Max(item => item.portfolio.NombreCartera) ?? string.Empty,
                grouped.Max(item => item.portfolio.UnidadNegocioOrigen),
                grouped.Key.CodigoCampana,
                grouped.Min(item => item.date.FechaCalendario),
                grouped.Max(item => item.date.FechaCalendario),
                grouped.Max(item => item.fact.FechaCarga)))
            .ToListAsync(cancellationToken);

        var parentBySubPortfolio = rawRows
            .GroupBy(row => row.IdSubCartera)
            .ToDictionary(
                group => group.Key,
                group => new PadreSubCartera(
                    group
                        .Select(item => item.NombreSubCartera)
                        .OrderByDescending(name => name, StringComparer.OrdinalIgnoreCase)
                        .First(),
                    group.Max(item => item.FechaActualizacionUtc)));

        var sortOrderBySubPortfolio = parentBySubPortfolio
            .OrderBy(item => item.Value.NombreSubCartera, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Key)
            .Select((item, index) => new { item.Key, SortOrder = (long)index + 1 })
            .ToDictionary(item => item.Key, item => item.SortOrder);

        return rawRows
            .Select(row => new CarteraFiltroSubcarteraCampanaDbFila
            {
                IdSubCartera = row.IdSubCartera,
                NombreSubCartera = parentBySubPortfolio[row.IdSubCartera].NombreSubCartera,
                CodigoUnidadNegocio = NormalizarUnidadNegocio(row.CodigoUnidadNegocio),
                CodigoCampana = row.CodigoCampana,
                FechaDisponibleDesde = row.FechaDisponibleDesde,
                FechaDisponibleHasta = row.FechaDisponibleHasta,
                FechaActualizacionUtc = row.FechaActualizacionUtc,
                FechaActualizacionSubCarteraUtc = parentBySubPortfolio[row.IdSubCartera].FechaActualizacionUtc,
                OrdenSubCartera = sortOrderBySubPortfolio[row.IdSubCartera]
            })
            .OrderByDescending(row => row.CodigoCampana, StringComparer.OrdinalIgnoreCase)
            .ThenBy(row => row.IdSubCartera)
            .ToArray();
    }

    private static async Task<IReadOnlyList<CarteraFiltroSupervisorContextoDbFila>> CargarContextosSupervisorAsync(
        AnaliticaDbContext context,
        int claveCliente,
        CancellationToken cancellationToken)
    {
        var rawRows = await (
            from attribution in context.AtribucionesDiariasSupervisorAsesorAnalitica.AsNoTracking()
            join campana in context.CampanasAnalitica.AsNoTracking()
                on attribution.ClaveCampana equals campana.ClaveCampana
            where attribution.ClaveCliente == claveCliente
                  && attribution.ClaveSupervisor != null
            group attribution by new
            {
                attribution.ClaveSupervisor,
                attribution.ClaveCartera,
                campana.CodigoCampana
            }
            into grouped
            select new ContextoSupervisorCrudo(
                grouped.Key.ClaveSupervisor!.Value,
                grouped.Max(item => item.NombreSupervisor) ?? string.Empty,
                grouped.Key.ClaveCartera,
                grouped.Key.CodigoCampana,
                grouped.Min(item => item.FechaCalendario),
                grouped.Max(item => item.FechaCalendario),
                grouped.Max(item => item.FechaCarga)))
            .ToListAsync(cancellationToken);

        var parentBySupervisor = rawRows
            .GroupBy(row => row.IdSupervisor)
            .ToDictionary(
                group => group.Key,
                group => new PadreSupervisor(
                    group
                        .Select(item => item.NombreSupervisor)
                        .OrderByDescending(name => name, StringComparer.OrdinalIgnoreCase)
                        .First(),
                    group.Max(item => item.FechaActualizacionUtc)));

        var sortOrderBySupervisor = parentBySupervisor
            .OrderBy(item => item.Value.NombreSupervisor, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Key)
            .Select((item, index) => new { item.Key, SortOrder = (long)index + 1 })
            .ToDictionary(item => item.Key, item => item.SortOrder);

        return rawRows
            .Select(row => new CarteraFiltroSupervisorContextoDbFila
            {
                IdSupervisor = row.IdSupervisor,
                NombreSupervisor = parentBySupervisor[row.IdSupervisor].NombreSupervisor,
                IdSubCartera = row.IdSubCartera,
                CodigoCampana = row.CodigoCampana,
                FechaDisponibleDesde = row.FechaDisponibleDesde,
                FechaDisponibleHasta = row.FechaDisponibleHasta,
                FechaActualizacionUtc = row.FechaActualizacionUtc,
                FechaActualizacionSupervisorUtc = parentBySupervisor[row.IdSupervisor].FechaActualizacionUtc,
                OrdenSupervisor = sortOrderBySupervisor[row.IdSupervisor]
            })
            .OrderByDescending(row => row.CodigoCampana, StringComparer.OrdinalIgnoreCase)
            .ThenBy(row => row.IdSubCartera)
            .ThenBy(row => row.IdSupervisor)
            .ToArray();
    }

    private static string? NormalizarUnidadNegocio(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private sealed record DisponibilidadCampana(
        int ClaveCampana,
        DateTime FechaDisponibleDesde,
        DateTime FechaDisponibleHasta,
        DateTime? FechaActualizacionUtc);

    private sealed record ContextoSubCarteraCrudo(
        long IdSubCartera,
        string NombreSubCartera,
        string? CodigoUnidadNegocio,
        string CodigoCampana,
        DateTime FechaDisponibleDesde,
        DateTime FechaDisponibleHasta,
        DateTime? FechaActualizacionUtc);

    private sealed record PadreSubCartera(
        string NombreSubCartera,
        DateTime? FechaActualizacionUtc);

    private sealed record ContextoSupervisorCrudo(
        int IdSupervisor,
        string NombreSupervisor,
        long IdSubCartera,
        string CodigoCampana,
        DateTime FechaDisponibleDesde,
        DateTime FechaDisponibleHasta,
        DateTime? FechaActualizacionUtc);

    private sealed record PadreSupervisor(
        string NombreSupervisor,
        DateTime? FechaActualizacionUtc);
}
