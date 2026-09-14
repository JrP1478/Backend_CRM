using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class EvolucionCarteraResponseMapper
{
    public static EvolucionCarteraResponse Map(
        EvolucionCarteraContexto context,
        RangoEvolucionCartera range,
        IReadOnlyList<EvolucionCarteraDbFila> rows)
    {
        var points = rows
            .Select(row => new EvolucionCarteraPunto(
                DateOnly.FromDateTime(row.Periodo),
                row.CarteraAsignada,
                row.CarteraGestionada,
                row.CarteraPendiente,
                row.MontoRecuperado))
            .ToArray();

        var latestLoadedAt = rows
            .Where(row => row.FechaCargaUtc.HasValue)
            .Select(row => row.FechaCargaUtc!.Value)
            .DefaultIfEmpty()
            .Max();

        return new EvolucionCarteraResponse(
            new EvolucionCarteraCampana(
                context.CodigoCampana,
                context.NombreCampana),
            new EvolucionCarteraPeriodo(
                range.FechaDesde,
                range.FechaHasta),
            latestLoadedAt == default
                ? null
                : ConvertirOffsetUtc(latestLoadedAt),
            points);
    }

    private static DateTimeOffset ConvertirOffsetUtc(DateTime value)
    {
        var utc = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        return new DateTimeOffset(utc);
    }
}
