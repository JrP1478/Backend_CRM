using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record RendimientoCampanaCarteraRequest(
    string? Campana,
    long? IdSubCartera,
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
        out RendimientoCampanaCarteraRequest? request,
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

        var idSubCarteraParseado = ParsearIdSubCartera(idSubCartera, errors);
        var fechaDesdeParseada = ParsearFecha("fechaDesde", fechaDesde, errors);
        var fechaHastaParseada = ParsearFecha("fechaHasta", fechaHasta, errors);

        if (!string.IsNullOrWhiteSpace(idSupervisor))
        {
            errors["idSupervisor"] =
            [
                "idSupervisor no aplica a Campana Performance: assigned, progress, " +
                "contactability y meta no son métricas atribuibles a supervisor."
            ];
        }

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

        request = new RendimientoCampanaCarteraRequest(
            campanaNormalizada,
            idSubCarteraParseado,
            fechaDesdeParseada,
            fechaHastaParseada);

        return true;
    }

    private static long? ParsearIdSubCartera(
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

        errors["idSubCartera"] =
        ["idSubCartera debe ser un entero positivo."];

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
