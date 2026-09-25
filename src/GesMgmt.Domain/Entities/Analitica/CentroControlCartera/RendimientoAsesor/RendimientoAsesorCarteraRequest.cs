using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record RendimientoAsesorCarteraRequest(
    string? Campana,
    long? IdSubCartera,
    int? IdSupervisor,
    DateOnly? FechaDesde,
    DateOnly? FechaHasta)
{
    public string? UnidadNegocio { get; init; }

    private static readonly Regex PatronCampana = new(
        @"^\d{4}-(0[1-9]|1[0-2])$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool IntentarCrear(
        string? campana,
        string? idSubCartera,
        string? idSupervisor,
        string? fechaDesde,
        string? fechaHasta,
        out RendimientoAsesorCarteraRequest? request,
        out Dictionary<string, string[]> errors)
    {
        errors = [];

        var campanaNormalizada = string.IsNullOrWhiteSpace(campana)
            ? null
            : campana.Trim();

        if (campanaNormalizada is not null
            && !PatronCampana.IsMatch(campanaNormalizada))
        {
            errors["campana"] =
            ["campana debe tener formato YYYY-MM."];
        }

        var idSubCarteraParseado = ParsearLongPositivo(
            "idSubCartera",
            idSubCartera,
            errors);

        var parsedSupervisorId = ParsearEnteroPositivo(
            "idSupervisor",
            idSupervisor,
            errors);

        var fechaDesdeParseada = ParsearFecha("fechaDesde", fechaDesde, errors);
        var fechaHastaParseada = ParsearFecha("fechaHasta", fechaHasta, errors);

        if (fechaDesdeParseada.HasValue
            && fechaHastaParseada.HasValue
            && fechaDesdeParseada.Value > fechaHastaParseada.Value)
        {
            errors["rangoFechas"] =
            ["fechaDesde no puede ser posterior a fechaHasta."];
        }

        if (errors.Count > 0)
        {
            request = null;
            return false;
        }

        request = new RendimientoAsesorCarteraRequest(
            campanaNormalizada,
            idSubCarteraParseado,
            parsedSupervisorId,
            fechaDesdeParseada,
            fechaHastaParseada);

        return true;
    }

    private static long? ParsearLongPositivo(
        string fieldName,
        string? value,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (long.TryParse(
                value.Trim(),
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var parsed)
            && parsed > 0)
        {
            return parsed;
        }

        errors[fieldName] =
        [$"{fieldName} debe ser un entero positivo."];

        return null;
    }

    private static int? ParsearEnteroPositivo(
        string fieldName,
        string? value,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (int.TryParse(
                value.Trim(),
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var parsed)
            && parsed > 0)
        {
            return parsed;
        }

        errors[fieldName] =
        [$"{fieldName} debe ser un entero positivo."];

        return null;
    }

    private static DateOnly? ParsearFecha(
        string fieldName,
        string? value,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (DateOnly.TryParseExact(
                value.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsed))
        {
            return parsed;
        }

        errors[fieldName] =
        [$"{fieldName} debe tener formato YYYY-MM-DD."];

        return null;
    }
}
