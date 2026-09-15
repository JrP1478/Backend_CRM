using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record PromesasCarteraRequest(
    string? Campana,
    long? IdSubCartera)
{
    public string? UnidadNegocio { get; init; }

    private static readonly Regex PatronCampana = new(
        @"^\d{4}-(0[1-9]|1[0-2])$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool IntentarCrear(
        string? campana,
        string? idSubCartera,
        string? fechaDesde,
        string? fechaHasta,
        out PromesasCarteraRequest? request,
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

        if (!string.IsNullOrWhiteSpace(fechaDesde))
        {
            errors["fechaDesde"] =
            ["fechaDesde no aplica al estado operativo actual de promesas."];
        }

        if (!string.IsNullOrWhiteSpace(fechaHasta))
        {
            errors["fechaHasta"] =
            ["fechaHasta no aplica al estado operativo actual de promesas."];
        }

        if (errors.Count > 0)
        {
            request = null;
            return false;
        }

        request = new PromesasCarteraRequest(
            campanaNormalizada,
            idSubCarteraParseado);
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
}
