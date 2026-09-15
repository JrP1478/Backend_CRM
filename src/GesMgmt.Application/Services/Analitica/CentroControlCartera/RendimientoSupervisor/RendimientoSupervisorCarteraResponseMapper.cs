using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class RendimientoSupervisorCarteraResponseMapper
{
    public static RendimientoSupervisorCarteraResponse Map(
        IReadOnlyList<RendimientoSupervisorCarteraDbFila> rows)
    {
        DateOnly? fechaDesde = rows.Count == 0
            ? null
            : DateOnly.FromDateTime(rows.Min(row => row.FechaDesde));

        DateOnly? fechaHasta = rows.Count == 0
            ? null
            : DateOnly.FromDateTime(rows.Max(row => row.FechaHasta));

        var fechaActualizacionUtc = rows
            .Where(row => row.FechaActualizacionUtc.HasValue)
            .Select(row => row.FechaActualizacionUtc!.Value)
            .DefaultIfEmpty()
            .Max();

        var supervisores = rows
            .Select(row => new RendimientoSupervisorCarteraItem(
                row.IdSupervisor,
                row.NombreSupervisor,
                row.CantidadAsesores,
                row.CantidadGestiones,
                row.CantidadDeudoresGestionados,
                ConvertirPorcentaje(row.TasaContactoDirecto),
                ConvertirPorcentaje(row.TasaCierre),
                row.CantidadPromesas,
                ConvertirPorcentaje(row.TasaCumplimientoPromesa),
                row.CantidadPagos,
                row.MontoRecuperadoAtribuible))
            .ToArray();

        return new RendimientoSupervisorCarteraResponse(
            fechaDesde,
            fechaHasta,
            fechaActualizacionUtc == default
                ? null
                : ConvertirOffsetUtc(fechaActualizacionUtc),
            supervisores);
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
