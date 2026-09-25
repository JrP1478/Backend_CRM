using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class ResumenCarteraResponseMapper
{
    public static ResumenCarteraResponse Map(
        ResumenCarteraContexto context,
        RangoResumenCartera range,
        ResumenCarteraDbFila row)
    {
        if (!row.FechaCorte.HasValue)
        {
            throw new InvalidOperationException(
                "No se puede construir el resumen sin snapshot real.");
        }

        return new ResumenCarteraResponse(
            new ResumenCarteraCampana(
                context.CodigoCampana,
                context.NombreCampana),
            new ResumenCarteraPeriodo(
                range.FechaDesde,
                range.FechaHasta,
                DateOnly.FromDateTime(row.FechaCorte.Value)),
            ConvertirOffsetUtc(row.FechaActualizacionUtc),
            new ResumenCarteraVigencia(
                ConvertirOffsetPeru(row.FechaCorteOperacionLocal),
                ConvertirOffsetUtc(row.FechaActualizacionBaseCarteraUtc),
                ConvertirOffsetUtc(row.FechaActualizacionDatosUtc)),
            new ResumenCarteraMetricas(
                row.CarteraAsignada,
                row.CarteraGestionada,
                row.CarteraPendiente,
                row.CantidadGestiones,
                row.IntensidadGestion,
                row.MontoRecuperado,
                ConvertirPorcentaje(row.TasaContactabilidad),
                ConvertirPorcentaje(row.TasaContactoDirecto),
                ConvertirPorcentaje(row.TasaCierre),
                row.CantidadPromesas,
                ConvertirPorcentaje(row.TasaCumplimientoPromesa),
                row.CantidadPagos));
    }

    private static decimal? ConvertirPorcentaje(decimal? value)
    {
        return value * 100m;
    }

    private static DateTimeOffset? ConvertirOffsetUtc(DateTime? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        var utc = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
        return new DateTimeOffset(utc);
    }

    private static DateTimeOffset? ConvertirOffsetPeru(DateTime? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        var local = DateTime.SpecifyKind(
            value.Value,
            DateTimeKind.Unspecified);

        return new DateTimeOffset(
            local,
            TimeSpan.FromHours(-5));
    }
}
