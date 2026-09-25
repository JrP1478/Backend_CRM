using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class SeguimientoPromesasCarteraResponseMapper
{
    private static readonly IReadOnlyDictionary<string, string> EtiquetasEstado =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["pendiente"] = "Pendiente",
            ["parcial"] = "Pago parcial",
            ["cumplida"] = "Cumplida",
            ["incumplida"] = "Incumplida",
            ["pagada-fuera-plazo"] = "Pagada fuera de plazo"
        };

    private static readonly IReadOnlyDictionary<string, string> EtiquetasContacto =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["directo"] = "Contacto directo",
            ["indirecto"] = "Contacto indirecto",
            ["sin-contacto"] = "Sin contacto",
            ["sin-gestion"] = "Sin gestión"
        };

    public static SeguimientoPromesasCarteraResponse Map(
        PromesasCarteraContexto context,
        SeguimientoPromesasCarteraConsultaResult result)
    {
        return new SeguimientoPromesasCarteraResponse(
            new PromesasCarteraCampana(context.CodigoCampana, context.NombreCampana),
            result.FechaVencimiento,
            result.Resumen.FechaCorte.HasValue
                ? DateOnly.FromDateTime(result.Resumen.FechaCorte.Value)
                : null,
            ConvertirOffsetUtc(result.Resumen.FechaActualizacionUtc),
            new SeguimientoPromesasCarteraResumen(
                result.Resumen.CantidadPromesas,
                result.Resumen.MontoPromesa,
                result.Resumen.MontoPagado,
                result.Resumen.MontoPendiente),
            result.Estado
                .OrderBy(item => ObtenerOrdenEstado(item.ClaveEstado))
                .Select(item => new SeguimientoPromesasCarteraEstadoRango(
                    item.ClaveEstado,
                    EtiquetasEstado.GetValueOrDefault(item.ClaveEstado, item.ClaveEstado),
                    item.CantidadPromesas,
                    item.MontoPromesa,
                    item.MontoPagado,
                    item.MontoPendiente))
                .ToArray(),
            result.Paginacion,
            result.Elementos.Select(item => new SeguimientoPromesaCarteraItem(
                item.IdPromesa,
                item.IdDeudor,
                item.NombreDeudor,
                item.FechaVencimiento.HasValue
                    ? DateOnly.FromDateTime(item.FechaVencimiento.Value)
                    : null,
                item.MontoPromesa,
                item.MontoPagado,
                item.MontoPendiente,
                item.FechaUltimoPago.HasValue
                    ? DateOnly.FromDateTime(item.FechaUltimoPago.Value)
                    : null,
                item.ClaveEstado,
                item.Gestionado,
                item.CantidadGestiones,
                item.CantidadLlamadas,
                item.ClaveContacto,
                EtiquetasContacto.GetValueOrDefault(item.ClaveContacto, item.ClaveContacto),
                item.ConfirmoPago,
                item.FechaUltimaGestion,
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
            "cumplida" => 3,
            "incumplida" => 4,
            "pagada-fuera-plazo" => 5,
            _ => 6
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
