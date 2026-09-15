using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record AvanceMetaCarteraRequest(
    string? Campana,
    long? IdSubCartera,
    DateOnly? FechaHasta)
{
    public string? UnidadNegocio { get; init; }

    private static readonly Regex PatronCampana = new(
        @"^\d{4}-(0[1-9]|1[0-2])$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool IntentarCrear(
        string? campana,
        string? idSubCartera,
        string? fechaHasta,
        out AvanceMetaCarteraRequest? request,
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
        var fechaHastaParseada = ParsearFecha("fechaHasta", fechaHasta, errors);

        if (errors.Count > 0)
        {
            request = null;
            return false;
        }

        request = new AvanceMetaCarteraRequest(
            campanaNormalizada,
            idSubCarteraParseado,
            fechaHastaParseada);

        return true;
    }

    public bool IntentarResolverFechaHasta(
        AvanceMetaCarteraContexto context,
        out DateOnly fechaHasta,
        out Dictionary<string, string[]> errors)
    {
        errors = [];

        fechaHasta = FechaHasta
            ?? context.LatestProgressDate
            ?? context.FechaFin;

        if (fechaHasta < context.FechaInicio || fechaHasta > context.FechaFin)
        {
            errors["fechaHasta"] =
            [$"fechaHasta debe estar entre {context.FechaInicio:yyyy-MM-dd} y {context.FechaFin:yyyy-MM-dd}."];
        }

        return errors.Count == 0;
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
