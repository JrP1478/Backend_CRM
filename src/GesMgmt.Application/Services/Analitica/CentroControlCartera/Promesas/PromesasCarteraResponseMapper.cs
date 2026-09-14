using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class PromesasCarteraResponseMapper
{
    public static PromesasCarteraResponse Map(
        PromesasCarteraContexto context,
        PromesasCarteraDbFila row)
    {
        return new PromesasCarteraResponse(
            new PromesasCarteraCampana(
                context.CodigoCampana,
                context.NombreCampana),
            ConvertirOffsetUtc(row.FechaActualizacionUtc),
            new PromesaCarteraEstadoMetricas(
                row.CantidadVenceHoy,
                row.MontoVenceHoy,
                row.CantidadVencidas,
                ConvertirPorcentaje(row.TasaCumplimiento)));
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
