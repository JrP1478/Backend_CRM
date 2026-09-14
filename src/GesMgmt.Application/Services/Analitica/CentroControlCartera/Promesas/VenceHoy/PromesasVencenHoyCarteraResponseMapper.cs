using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class PromesasVencenHoyCarteraResponseMapper
{
    private static readonly IReadOnlyDictionary<string, string> StatusLabels =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["pendiente"] = "Pendiente",
            ["parcial"] = "Pago parcial",
            ["cubierta"] = "Cubierta"
        };

    public static PromesasVencenHoyCarteraResponse Map(
        PromesasCarteraContexto context,
        PromesasVencenHoyCarteraConsultaResult result)
    {
        return new PromesasVencenHoyCarteraResponse(
            new PromesasCarteraCampana(context.CodigoCampana, context.NombreCampana),
            result.Resumen.FechaCorte.HasValue
                ? DateOnly.FromDateTime(result.Resumen.FechaCorte.Value)
                : null,
            ConvertirOffsetUtc(result.Resumen.FechaActualizacionUtc),
            new PromesasVencenHoyCarteraResumen(
                result.Resumen.CantidadVenceHoy,
                result.Resumen.MontoVenceHoy,
                result.Resumen.MontoPagado,
                result.Resumen.MontoPendiente),
            result.Estado
                .OrderBy(item => ObtenerOrdenEstado(item.ClaveEstado))
                .Select(item => new PromesasVencenHoyCarteraEstadoRango(
                    item.ClaveEstado,
                    StatusLabels.GetValueOrDefault(item.ClaveEstado, item.ClaveEstado),
                    item.CantidadPromesas,
                    item.MontoPromesa,
                    item.MontoPagado,
                    item.MontoPendiente))
                .ToArray(),
            result.Paginacion,
            result.Elementos.Select(item => new PromesaVenceHoyCarteraItem(
                item.IdPromesa,
                item.IdDeudor,
                item.MontoPromesa,
                item.MontoPagado,
                item.MontoPendiente,
                item.FechaUltimoPago.HasValue
                    ? DateOnly.FromDateTime(item.FechaUltimoPago.Value)
                    : null,
                item.ClaveEstado,
                item.IdAsesor,
                item.NombreAsesor,
                item.IdSupervisor,
                item.NombreSupervisor)).ToArray());
    }

    private static int ObtenerOrdenEstado(string claveEstado) =>
        claveEstado switch
        {
            "pendiente" => 1,
            "parcial" => 2,
            "cubierta" => 3,
            _ => 4
        };

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
