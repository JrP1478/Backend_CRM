using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class RendimientoCampanaCarteraResponseMapper
{
    public static RendimientoCampanaCarteraResponse Map(
        IReadOnlyList<RendimientoCampanaCarteraDbFila> rows)
    {
        var elementos = rows
            .Select(row => new RendimientoCampanaCarteraItem(
                row.CodigoCampana,
                row.NombreCampana,
                DateOnly.FromDateTime(row.FechaDesde),
                DateOnly.FromDateTime(row.FechaHasta),
                DateOnly.FromDateTime(row.FechaCorte),
                row.CarteraAsignada,
                row.CarteraGestionada,
                row.CarteraPendiente,
                ConvertirPorcentaje(row.TasaAvance),
                row.CantidadGestiones,
                ConvertirPorcentaje(row.TasaContactabilidad),
                ConvertirPorcentaje(row.TasaContactoDirecto),
                ConvertirPorcentaje(row.TasaCierre),
                row.CantidadPromesas,
                ConvertirPorcentaje(row.TasaCumplimientoPromesa),
                row.CantidadPagos,
                row.MontoRecuperado,
                row.MontoMeta))
            .ToArray();

        var fechaActualizacionUtc = rows
            .Where(row => row.FechaActualizacionUtc.HasValue)
            .Select(row => row.FechaActualizacionUtc!.Value)
            .DefaultIfEmpty()
            .Max();

        return new RendimientoCampanaCarteraResponse(
            fechaActualizacionUtc == default
                ? null
                : ConvertirOffsetUtc(fechaActualizacionUtc),
            elementos);
    }

    private static decimal? ConvertirPorcentaje(decimal? value)
    {
        return value * 100m;
    }

    private static DateTimeOffset ConvertirOffsetUtc(DateTime value)
    {
        var utc = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        return new DateTimeOffset(utc);
    }
}
