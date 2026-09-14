namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public static class OpcionesFiltroCarteraDbResultMapper
{
    public static OpcionesFiltroCarteraDbResult Map(
        IReadOnlyList<CarteraFiltroCampanaDbFila> campanas,
        IReadOnlyList<CarteraFiltroSubcarteraCampanaDbFila> subPortfolioContexts,
        IReadOnlyList<CarteraFiltroSupervisorContextoDbFila> supervisorContexts) =>
        new(
            campanas,
            ConstruirSubCarteras(subPortfolioContexts),
            subPortfolioContexts,
            ConstruirSupervisores(supervisorContexts),
            supervisorContexts)
        {
            UnidadesNegocio = ConstruirUnidadesNegocio(subPortfolioContexts)
        };

    public static OpcionesFiltroCarteraDbResult AplicarAlcance(
        OpcionesFiltroCarteraDbResult source,
        string? unidadNegocio)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (string.IsNullOrWhiteSpace(unidadNegocio))
        {
            return source;
        }

        var scopedContexts = source.SubPortfolioCampaigns
            .Where(item => string.Equals(
                item.CodigoUnidadNegocio?.Trim(),
                unidadNegocio,
                StringComparison.OrdinalIgnoreCase))
            .ToArray();

        var scopedCampaignsBySubPortfolio = scopedContexts
            .GroupBy(item => item.IdSubCartera)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(item => item.CodigoCampana)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase));

        var scopedSupervisorContexts = source.SupervisorContexts
            .Where(item =>
                scopedCampaignsBySubPortfolio.TryGetValue(
                    item.IdSubCartera,
                    out var campaignCodes)
                && campaignCodes.Contains(item.CodigoCampana))
            .ToArray();

        return new OpcionesFiltroCarteraDbResult(
            ConstruirCampanas(source.Campanas, scopedContexts),
            ConstruirSubCarteras(scopedContexts),
            scopedContexts,
            ConstruirSupervisores(scopedSupervisorContexts),
            scopedSupervisorContexts)
        {
            UnidadesNegocio = source.UnidadesNegocio
        };
    }

    private static IReadOnlyList<CarteraFiltroCampanaDbFila> ConstruirCampanas(
        IReadOnlyList<CarteraFiltroCampanaDbFila> campanas,
        IReadOnlyList<CarteraFiltroSubcarteraCampanaDbFila> contexts)
    {
        var availabilityByCampaign = contexts
            .GroupBy(item => item.CodigoCampana, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => new
                {
                    FechaDisponibleDesde = group.Min(item => item.FechaDisponibleDesde),
                    FechaDisponibleHasta = group.Max(item => item.FechaDisponibleHasta),
                    FechaActualizacionUtc = group
                        .Where(item => item.FechaActualizacionUtc.HasValue)
                        .Select(item => item.FechaActualizacionUtc!.Value)
                        .DefaultIfEmpty()
                        .Max()
                },
                StringComparer.OrdinalIgnoreCase);

        return campanas
            .Where(item => availabilityByCampaign.ContainsKey(item.CodigoCampana))
            .Select(item =>
            {
                var availability = availabilityByCampaign[item.CodigoCampana];
                return new CarteraFiltroCampanaDbFila
                {
                    CodigoCampana = item.CodigoCampana,
                    NombreCampana = item.NombreCampana,
                    FechaInicio = item.FechaInicio,
                    FechaFin = item.FechaFin,
                    FechaDisponibleDesde = availability.FechaDisponibleDesde,
                    FechaDisponibleHasta = availability.FechaDisponibleHasta,
                    FechaActualizacionUtc = availability.FechaActualizacionUtc == default
                        ? null
                        : availability.FechaActualizacionUtc
                };
            })
            .ToArray();
    }

    private static IReadOnlyList<string> ConstruirUnidadesNegocio(
        IReadOnlyList<CarteraFiltroSubcarteraCampanaDbFila> contexts) =>
        contexts
            .Select(item => item.CodigoUnidadNegocio)
            .Where(static value => !string.IsNullOrWhiteSpace(value))
            .Select(static value => value!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(static value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static IReadOnlyList<CarteraFiltroSubcarteraDbFila> ConstruirSubCarteras(
        IReadOnlyList<CarteraFiltroSubcarteraCampanaDbFila> contexts) =>
        contexts
            .GroupBy(item => item.IdSubCartera)
            .Select(group =>
            {
                var parent = group.First();
                return new
                {
                    Row = new CarteraFiltroSubcarteraDbFila
                    {
                        IdSubCartera = group.Key,
                        NombreSubCartera = parent.NombreSubCartera,
                        FechaActualizacionUtc = parent.FechaActualizacionSubCarteraUtc
                    },
                    SortOrder = parent.OrdenSubCartera
                };
            })
            .OrderBy(item => item.SortOrder)
            .Select(item => item.Row)
            .ToArray();

    private static IReadOnlyList<CarteraFiltroSupervisorDbFila> ConstruirSupervisores(
        IReadOnlyList<CarteraFiltroSupervisorContextoDbFila> contexts) =>
        contexts
            .GroupBy(item => item.IdSupervisor)
            .Select(group =>
            {
                var parent = group.First();
                return new
                {
                    Row = new CarteraFiltroSupervisorDbFila
                    {
                        IdSupervisor = group.Key,
                        NombreSupervisor = parent.NombreSupervisor,
                        FechaActualizacionUtc = parent.FechaActualizacionSupervisorUtc
                    },
                    SortOrder = parent.OrdenSupervisor
                };
            })
            .OrderBy(item => item.SortOrder)
            .Select(item => item.Row)
            .ToArray();
}
