using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class AvanceMetaCarteraResponseMapper
{
    public static AvanceMetaCarteraResponse Map(
        AvanceMetaCarteraContexto context,
        DateOnly fechaHasta,
        AvanceMetaCarteraDbFila? row)
    {
        var hasTarget = row?.MontoMetaMensual is not null;

        return new AvanceMetaCarteraResponse(
            new AvanceMetaCarteraCampana(
                context.CodigoCampana,
                context.NombreCampana),
            new AvanceMetaCarteraPeriodo(
                fechaHasta,
                row is null
                    ? null
                    : DateOnly.FromDateTime(row.FechaCorte)),
            ConvertirOffsetUtc(row?.FechaActualizacionUtc),
            hasTarget
                ? new AvanceMetaCarteraMetricas(
                    row!.MontoMetaMensual!.Value,
                    row.MontoEsperadoFecha ?? 0m,
                    ConvertirPorcentaje(row.TasaCumplimientoMeta),
                    ConvertirPorcentaje(row.TasaCumplimientoRitmo),
                    row.MontoBrecha ?? 0m,
                    ConvertirPorcentaje(row.TasaBrecha))
                : null);
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
}
