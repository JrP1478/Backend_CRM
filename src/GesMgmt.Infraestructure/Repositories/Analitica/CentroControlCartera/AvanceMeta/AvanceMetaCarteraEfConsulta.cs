using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal static class AvanceMetaCarteraEfConsulta
{
    public static async Task<AvanceMetaCarteraContexto?> ResolverContextoAsync(
        AnaliticaDbContext context,
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        bool includeClientLevelTarget,
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

        if (unidadNegocio is null
            && await TieneAlcanceUnidadNegocioAmbiguoAsync(
                context,
                claveCliente.Value,
                cancellationToken))
        {
            return null;
        }

        var candidateCampaigns = context.CampanasAnalitica
            .AsNoTracking()
            .Where(campana => campana.ClaveCliente == claveCliente.Value);

        if (codigoCampana is not null)
        {
            candidateCampaigns = candidateCampaigns.Where(
                campana => campana.CodigoCampana == codigoCampana);
        }
        else if (unidadNegocio is not null)
        {
            var latestCampaignKey = await context.CampanasAnalitica
                .AsNoTracking()
                .Where(campana =>
                    campana.ClaveCliente == claveCliente.Value
                    && context.HechosDiariosCarteraAnalitica.Any(fact =>
                        fact.ClaveCliente == campana.ClaveCliente
                        && fact.ClaveCampana == campana.ClaveCampana))
                .OrderByDescending(campana => campana.FechaInicio)
                .ThenByDescending(campana => campana.ClaveCampana)
                .Select(campana => (int?)campana.ClaveCampana)
                .FirstOrDefaultAsync(cancellationToken);

            if (!latestCampaignKey.HasValue)
            {
                return null;
            }

            candidateCampaigns = candidateCampaigns.Where(
                campana => campana.ClaveCampana == latestCampaignKey.Value);
        }

        var campanas = await candidateCampaigns.ToListAsync(cancellationToken);
        if (campanas.Count == 0)
        {
            return null;
        }

        var campaignKeys = campanas
            .Select(campana => campana.ClaveCampana)
            .ToArray();

        var latestTargetDates = includeClientLevelTarget
            ? await CargarUltimasFechasMetaAsync(
                context,
                claveCliente.Value,
                campaignKeys,
                cancellationToken)
            : new Dictionary<int, DateTime?>();

        var latestPortfolioDates = await CargarUltimasFechasCarteraAsync(
            context,
            claveCliente.Value,
            campaignKeys,
            idSubCartera,
            unidadNegocio,
            cancellationToken);

        var useClientLevelTarget = idSubCartera is null && includeClientLevelTarget;

        var selected = campanas
            .Select(campana => new CampanaSeleccionada(
                campana,
                useClientLevelTarget
                    ? latestTargetDates.GetValueOrDefault(campana.ClaveCampana)
                    : latestPortfolioDates.GetValueOrDefault(campana.ClaveCampana)))
            .Where(item => useClientLevelTarget || item.LatestProgressDate.HasValue)
            .OrderBy(item => item.LatestProgressDate.HasValue ? 0 : 1)
            .ThenByDescending(item => item.LatestProgressDate)
            .ThenByDescending(item => item.Campana.FechaInicio)
            .ThenByDescending(item => item.Campana.ClaveCampana)
            .FirstOrDefault();

        if (selected is null)
        {
            return null;
        }

        return new AvanceMetaCarteraContexto(
            claveCliente.Value,
            selected.Campana.ClaveCampana,
            selected.Campana.CodigoCampana,
            selected.Campana.NombreCampana,
            DateOnly.FromDateTime(selected.Campana.FechaInicio),
            DateOnly.FromDateTime(selected.Campana.FechaFin),
            selected.LatestProgressDate.HasValue
                ? DateOnly.FromDateTime(selected.LatestProgressDate.Value)
                : null);
    }

    public static async Task<AvanceMetaCarteraDbFila?> ObtenerAsync(
        AnaliticaDbContext context,
        int claveCliente,
        int claveCampana,
        string? unidadNegocio,
        DateOnly fechaHasta,
        CancellationToken cancellationToken)
    {
        var dateToValue = fechaHasta.ToDateTime(TimeOnly.MinValue);

        var meta = await context.AvanceMetaCampanaAnalitica
            .AsNoTracking()
            .Where(row =>
                row.ClaveCliente == claveCliente
                && row.ClaveCampana == claveCampana
                && row.FechaCalendario <= dateToValue)
            .OrderByDescending(row => row.FechaCalendario)
            .FirstOrDefaultAsync(cancellationToken);

        if (meta is null)
        {
            return null;
        }

        var montoRecuperado = await CargarMontoRecuperadoAsync(
            context,
            claveCliente,
            claveCampana,
            unidadNegocio,
            meta.FechaCalendario,
            cancellationToken);

        var montoMetaMensual = RedondearMonto(meta.MontoMetaRecuperacion);
        var montoEsperadoFecha = RedondearMonto(meta.MontoEsperadoAcumulado);
        var montoBrecha = montoEsperadoFecha.HasValue
            ? RedondearMonto(montoRecuperado - montoEsperadoFecha.Value)
            : null;

        return new AvanceMetaCarteraDbFila
        {
            FechaCorte = meta.FechaCalendario,
            MontoMetaMensual = montoMetaMensual,
            MontoEsperadoFecha = montoEsperadoFecha,
            TasaCumplimientoMeta = Dividir(montoRecuperado, montoMetaMensual),
            TasaCumplimientoRitmo = Dividir(montoRecuperado, montoEsperadoFecha),
            MontoBrecha = montoBrecha,
            TasaBrecha = montoBrecha.HasValue
                ? Dividir(montoBrecha.Value, montoEsperadoFecha)
                : null,
            FechaActualizacionUtc = meta.FechaCorteMetaOrigen
        };
    }

    private static async Task<bool> TieneAlcanceUnidadNegocioAmbiguoAsync(
        AnaliticaDbContext context,
        int claveCliente,
        CancellationToken cancellationToken)
    {
        var unidadesNegocio = await (
            from fact in context.HechosDiariosCarteraAnalitica.AsNoTracking()
            join portfolio in context.CarterasAnalitica.AsNoTracking()
                on fact.ClaveCartera equals portfolio.ClaveCartera
            where fact.ClaveCliente == claveCliente
            select portfolio.UnidadNegocioOrigen)
            .Distinct()
            .ToListAsync(cancellationToken);

        return unidadesNegocio
            .Select(NormalizarUnidadNegocio)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(2)
            .Count() > 1;
    }

    private static async Task<Dictionary<int, DateTime?>> CargarUltimasFechasMetaAsync(
        AnaliticaDbContext context,
        int claveCliente,
        IReadOnlyCollection<int> campaignKeys,
        CancellationToken cancellationToken) =>
        await context.AvanceMetaCampanaAnalitica
            .AsNoTracking()
            .Where(row =>
                row.ClaveCliente == claveCliente
                && campaignKeys.Contains(row.ClaveCampana))
            .GroupBy(row => row.ClaveCampana)
            .Select(group => new
            {
                ClaveCampana = group.Key,
                LatestProgressDate = (DateTime?)group.Max(row => row.FechaCalendario)
            })
            .ToDictionaryAsync(
                item => item.ClaveCampana,
                item => item.LatestProgressDate,
                cancellationToken);

    private static async Task<Dictionary<int, DateTime?>> CargarUltimasFechasCarteraAsync(
        AnaliticaDbContext context,
        int claveCliente,
        IReadOnlyCollection<int> campaignKeys,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken)
    {
        if (unidadNegocio is null)
        {
            return await context.MetricasDiariasCarteraAnalitica
                .AsNoTracking()
                .Where(row =>
                    row.ClaveCliente == claveCliente
                    && campaignKeys.Contains(row.ClaveCampana)
                    && (!idSubCartera.HasValue
                        || row.ClaveCartera == idSubCartera.Value))
                .GroupBy(row => row.ClaveCampana)
                .Select(group => new
                {
                    ClaveCampana = group.Key,
                    LatestPortfolioDataDate = (DateTime?)group.Max(row => row.FechaCalendario)
                })
                .ToDictionaryAsync(
                    item => item.ClaveCampana,
                    item => item.LatestPortfolioDataDate,
                    cancellationToken);
        }

        return await (
            from row in context.MetricasDiariasCarteraAnalitica.AsNoTracking()
            join portfolio in context.CarterasAnalitica.AsNoTracking()
                on row.ClaveCartera equals portfolio.ClaveCartera
            where row.ClaveCliente == claveCliente
                  && campaignKeys.Contains(row.ClaveCampana)
                  && (!idSubCartera.HasValue
                      || row.ClaveCartera == idSubCartera.Value)
                  && portfolio.UnidadNegocioOrigen == unidadNegocio
            group row by row.ClaveCampana
            into grouped
            select new
            {
                ClaveCampana = grouped.Key,
                LatestPortfolioDataDate = (DateTime?)grouped.Max(row => row.FechaCalendario)
            })
            .ToDictionaryAsync(
                item => item.ClaveCampana,
                item => item.LatestPortfolioDataDate,
                cancellationToken);
    }

    private static async Task<decimal> CargarMontoRecuperadoAsync(
        AnaliticaDbContext context,
        int claveCliente,
        int claveCampana,
        string? unidadNegocio,
        DateTime fechaCorte,
        CancellationToken cancellationToken)
    {
        decimal? total;

        if (unidadNegocio is null)
        {
            total = await context.MetricasDiariasCarteraAnalitica
                .AsNoTracking()
                .Where(row =>
                    row.ClaveCliente == claveCliente
                    && row.ClaveCampana == claveCampana
                    && row.FechaCalendario <= fechaCorte)
                .SumAsync(row => row.MontoRecuperadoDia, cancellationToken);
        }
        else
        {
            total = await (
                from row in context.MetricasDiariasCarteraAnalitica.AsNoTracking()
                join portfolio in context.CarterasAnalitica.AsNoTracking()
                    on row.ClaveCartera equals portfolio.ClaveCartera
                where row.ClaveCliente == claveCliente
                      && row.ClaveCampana == claveCampana
                      && row.FechaCalendario <= fechaCorte
                      && portfolio.UnidadNegocioOrigen == unidadNegocio
                select row.MontoRecuperadoDia)
                .SumAsync(cancellationToken);
        }

        return RedondearMonto(total) ?? 0m;
    }

    private static decimal? Dividir(decimal numerator, decimal? denominator)
    {
        if (!denominator.HasValue || denominator.Value == 0m)
        {
            return null;
        }

        return decimal.Round(
            numerator / denominator.Value,
            6,
            MidpointRounding.AwayFromZero);
    }

    private static decimal? RedondearMonto(decimal? value) =>
        value.HasValue
            ? decimal.Round(value.Value, 4, MidpointRounding.AwayFromZero)
            : null;

    private static string? NormalizarUnidadNegocio(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private sealed record CampanaSeleccionada(
        DimensionCampanaAnalitica Campana,
        DateTime? LatestProgressDate);
}
