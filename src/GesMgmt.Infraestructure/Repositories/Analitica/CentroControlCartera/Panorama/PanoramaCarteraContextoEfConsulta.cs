using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal static class PanoramaCarteraContextoEfConsulta
{
    public static async Task<PanoramaCarteraContexto?> EjecutarAsync(
        AnaliticaDbContext context,
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
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

        var latestDataByCampaign = await CargarUltimasFechasResumenAsync(
            context,
            claveCliente.Value,
            campaignKeys,
            idSubCartera,
            unidadNegocio,
            cancellationToken);

        var selected = campanas
            .Select(campana => new CampanaSeleccionada(
                campana,
                latestDataByCampaign.GetValueOrDefault(campana.ClaveCampana)))
            .Where(item =>
                (idSubCartera is null && unidadNegocio is null)
                || item.FechaUltimoDato.HasValue)
            .OrderBy(item => item.FechaUltimoDato.HasValue ? 0 : 1)
            .ThenByDescending(item => item.Campana.FechaInicio)
            .ThenByDescending(item => item.Campana.ClaveCampana)
            .FirstOrDefault();

        if (selected is null)
        {
            return null;
        }

        var subCarteraOperativaDisponible = idSubCartera is null
            || await TieneDatosSubCarteraOperativaAsync(
                context,
                claveCliente.Value,
                selected.Campana.ClaveCampana,
                idSubCartera.Value,
                unidadNegocio,
                cancellationToken);

        return new PanoramaCarteraContexto(
            new ResumenCarteraContexto(
                claveCliente.Value,
                selected.Campana.ClaveCampana,
                selected.Campana.CodigoCampana,
                selected.Campana.NombreCampana,
                DateOnly.FromDateTime(selected.Campana.FechaInicio),
                DateOnly.FromDateTime(selected.Campana.FechaFin),
                selected.FechaUltimoDato.HasValue
                    ? DateOnly.FromDateTime(selected.FechaUltimoDato.Value)
                    : null),
            subCarteraOperativaDisponible);
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

    private static async Task<Dictionary<int, DateTime?>> CargarUltimasFechasResumenAsync(
        AnaliticaDbContext context,
        int claveCliente,
        IReadOnlyCollection<int> campaignKeys,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken)
    {
        if (unidadNegocio is null)
        {
            return await context.EstadoResumenDiarioCarteraAnalitica
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
                    FechaUltimoDato = (DateTime?)group.Max(row => row.FechaCalendario)
                })
                .ToDictionaryAsync(
                    item => item.ClaveCampana,
                    item => item.FechaUltimoDato,
                    cancellationToken);
        }

        return await (
            from row in context.EstadoResumenDiarioCarteraAnalitica.AsNoTracking()
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
                FechaUltimoDato = (DateTime?)grouped.Max(row => row.FechaCalendario)
            })
            .ToDictionaryAsync(
                item => item.ClaveCampana,
                item => item.FechaUltimoDato,
                cancellationToken);
    }

    private static async Task<bool> TieneDatosSubCarteraOperativaAsync(
        AnaliticaDbContext context,
        int claveCliente,
        int claveCampana,
        long idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken)
    {
        if (unidadNegocio is null)
        {
            return await context.MetricasDiariasCarteraAnalitica
                .AsNoTracking()
                .AnyAsync(
                    row =>
                        row.ClaveCliente == claveCliente
                        && row.ClaveCampana == claveCampana
                        && row.ClaveCartera == idSubCartera,
                    cancellationToken);
        }

        return await (
            from row in context.MetricasDiariasCarteraAnalitica.AsNoTracking()
            join portfolio in context.CarterasAnalitica.AsNoTracking()
                on row.ClaveCartera equals portfolio.ClaveCartera
            where row.ClaveCliente == claveCliente
                  && row.ClaveCampana == claveCampana
                  && row.ClaveCartera == idSubCartera
                  && portfolio.UnidadNegocioOrigen == unidadNegocio
            select row.ClaveCartera)
            .AnyAsync(cancellationToken);
    }

    private static string? NormalizarUnidadNegocio(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private sealed record CampanaSeleccionada(
        DimensionCampanaAnalitica Campana,
        DateTime? FechaUltimoDato);
}
