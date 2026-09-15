using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class RendimientoAsesorCarteraResponseMapper
{
    public static RendimientoAsesorCarteraResponse Map(
        IReadOnlyList<RendimientoAsesorCarteraDbFila> rows)
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

        var advisors = rows
            .Select(row => new RendimientoAsesorCarteraItem(
                row.IdAsesor,
                NormalizarNombreAsesor(row.IdAsesor, row.NombreAsesor),
                row.IdSupervisorPeriodo,
                NormalizarNombreOpcional(row.NombreSupervisorPeriodo),
                row.IdSupervisorActual,
                NormalizarNombreOpcional(row.NombreSupervisorActual),
                row.CantidadGestiones,
                row.CantidadDeudoresGestionados,
                ConvertirPorcentaje(row.TasaContactoDirecto),
                ConvertirPorcentaje(row.TasaCierre),
                row.CantidadPromesas,
                row.CantidadPagos,
                row.MontoRecuperadoAtribuible))
            .ToArray();

        return new RendimientoAsesorCarteraResponse(
            fechaDesde,
            fechaHasta,
            fechaActualizacionUtc == default
                ? null
                : ConvertirOffsetUtc(fechaActualizacionUtc),
            advisors);
    }

    private static string NormalizarNombreAsesor(int idAsesor, string? nombreAsesor)
    {
        var normalized = NormalizarNombreOpcional(nombreAsesor);
        return normalized ?? $"Sin nombre (ID {idAsesor})";
    }

    private static string? NormalizarNombreOpcional(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
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
