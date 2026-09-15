using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class OpcionesFiltroCarteraResponseMapper
{
    public static OpcionesFiltroCarteraResponse Map(
        OpcionesFiltroCarteraDbResult source,
        int idClienteCrm,
        string? selectedBusinessUnit = null)
    {
        var scopedSource = OpcionesFiltroCarteraDbResultMapper.AplicarAlcance(
            source,
            selectedBusinessUnit);
        source = scopedSource;
        DateOnly? fechaDisponibleDesde = source.Campanas.Count == 0
            ? null
            : DateOnly.FromDateTime(
                source.Campanas.Min(item => item.FechaDisponibleDesde));

        DateOnly? fechaDisponibleHasta = source.Campanas.Count == 0
            ? null
            : DateOnly.FromDateTime(
                source.Campanas.Max(item => item.FechaDisponibleHasta));

        var fechaActualizacionUtc = EnumerarFechasActualizacion(source)
            .Where(value => value.HasValue)
            .Select(value => value!.Value)
            .DefaultIfEmpty()
            .Max();

        return new OpcionesFiltroCarteraResponse(
            fechaDisponibleDesde,
            fechaDisponibleHasta,
            fechaActualizacionUtc == default
                ? null
                : ConvertirOffsetUtc(fechaActualizacionUtc),
            new CarteraFiltroCarteraAlcance(idClienteCrm),
            source.UnidadesNegocio
                .Select(code => new CarteraFiltroUnidadNegocioOpcion(code, code))
                .ToArray(),
            selectedBusinessUnit,
            source.Campanas
                .Select(item => new CarteraFiltroCampanaOpcion(
                    item.CodigoCampana,
                    item.NombreCampana,
                    DateOnly.FromDateTime(item.FechaInicio),
                    DateOnly.FromDateTime(item.FechaFin),
                    DateOnly.FromDateTime(item.FechaDisponibleDesde),
                    DateOnly.FromDateTime(item.FechaDisponibleHasta)))
                .ToArray(),
            source.SubPortfolios
                .Select(item => new CarteraFiltroSubcarteraOpcion(
                    item.IdSubCartera,
                    item.NombreSubCartera))
                .ToArray(),
            source.Supervisores
                .Select(item => new CarteraFiltroSupervisorOpcion(
                    item.IdSupervisor,
                    item.NombreSupervisor))
                .ToArray(),
            new CarteraFiltroDisponibilidad(
                source.SubPortfolioCampaigns
                    .Select(item => new SubcarteraCampanaDisponibilidad(
                        item.IdSubCartera,
                        item.CodigoCampana,
                        DateOnly.FromDateTime(item.FechaDisponibleDesde),
                        DateOnly.FromDateTime(item.FechaDisponibleHasta)))
                    .ToArray(),
                source.SupervisorContexts
                    .Select(item => new CarteraSupervisorContextoDisponibilidad(
                        item.IdSupervisor,
                        item.IdSubCartera,
                        item.CodigoCampana,
                        DateOnly.FromDateTime(item.FechaDisponibleDesde),
                        DateOnly.FromDateTime(item.FechaDisponibleHasta)))
                    .ToArray()));
    }

    private static IEnumerable<DateTime?> EnumerarFechasActualizacion(
        OpcionesFiltroCarteraDbResult source)
    {
        return source.Campanas.Select(item => item.FechaActualizacionUtc)
            .Concat(source.SubPortfolios.Select(item => item.FechaActualizacionUtc))
            .Concat(source.SubPortfolioCampaigns.Select(item => item.FechaActualizacionUtc))
            .Concat(source.Supervisores.Select(item => item.FechaActualizacionUtc))
            .Concat(source.SupervisorContexts.Select(item => item.FechaActualizacionUtc));
    }

    private static DateTimeOffset ConvertirOffsetUtc(DateTime value)
    {
        var utc = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        return new DateTimeOffset(utc);
    }
}
