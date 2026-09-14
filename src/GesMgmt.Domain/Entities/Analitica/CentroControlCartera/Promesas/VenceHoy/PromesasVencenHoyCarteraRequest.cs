using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record PromesasVencenHoyCarteraRequest(
    string? Campana,
    long? IdSubCartera,
    int Pagina,
    int TamanoPagina,
    string? Estado,
    string OrdenarPor,
    string DireccionOrden)
{
    public string? UnidadNegocio { get; init; }

    private static readonly Regex PatronCampana = new(
        @"^\d{4}-(0[1-9]|1[0-2])$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly HashSet<string> EstadosPermitidos =
        new(StringComparer.OrdinalIgnoreCase) { "pendiente", "parcial", "cubierta" };

    private static readonly HashSet<string> ClavesOrdenPermitidas =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "idDeudor", "montoPromesa", "montoPagado", "montoPendiente",
            "etiquetaEstado", "fechaUltimoPago", "nombreAsesor", "nombreSupervisor"
        };

    public static bool IntentarCrear(
        string? campana,
        string? idSubCartera,
        out PromesasVencenHoyCarteraRequest? request,
        out Dictionary<string, string[]> errors) =>
        IntentarCrear(campana, idSubCartera, null, null, null, null, null, out request, out errors);

    public static bool IntentarCrear(
        string? campana,
        string? idSubCartera,
        string? pagina,
        string? tamanoPagina,
        out PromesasVencenHoyCarteraRequest? request,
        out Dictionary<string, string[]> errors) =>
        IntentarCrear(campana, idSubCartera, pagina, tamanoPagina, null, null, null, out request, out errors);

    public static bool IntentarCrear(
        string? campana,
        string? idSubCartera,
        string? pagina,
        string? tamanoPagina,
        string? estado,
        string? ordenarPor,
        string? direccionOrden,
        out PromesasVencenHoyCarteraRequest? request,
        out Dictionary<string, string[]> errors)
    {
        errors = [];

        var campanaNormalizada = Normalizar(campana);
        if (campanaNormalizada is not null && !PatronCampana.IsMatch(campanaNormalizada))
        {
            errors["campana"] = ["campana debe tener formato YYYY-MM."];
        }

        var idSubCarteraParseado = ParsearIdSubCartera(idSubCartera, errors);
        PromesasCarteraPaginado.Parsear(pagina, tamanoPagina, errors, out var paginaParseada, out var tamanoPaginaParseado);

        var estadoNormalizado = Normalizar(estado);
        if (estadoNormalizado is not null && !EstadosPermitidos.Contains(estadoNormalizado))
        {
            errors["estado"] = ["estado no es válido."];
        }

        var ordenarPorNormalizado = Normalizar(ordenarPor) ?? "montoPendiente";
        if (!ClavesOrdenPermitidas.Contains(ordenarPorNormalizado))
        {
            errors["ordenarPor"] = ["ordenarPor no es válido."];
        }

        var direccionOrdenNormalizada = (Normalizar(direccionOrden) ?? "desc").ToLowerInvariant();
        if (direccionOrdenNormalizada is not ("asc" or "desc"))
        {
            errors["direccionOrden"] = ["direccionOrden debe ser asc o desc."];
        }

        if (errors.Count > 0)
        {
            request = null;
            return false;
        }

        request = new PromesasVencenHoyCarteraRequest(
            campanaNormalizada,
            idSubCarteraParseado,
            paginaParseada,
            tamanoPaginaParseado,
            estadoNormalizado?.ToLowerInvariant(),
            ordenarPorNormalizado,
            direccionOrdenNormalizada);
        return true;
    }

    private static long? ParsearIdSubCartera(
        string? value,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;

        if (long.TryParse(value.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out var parsed)
            && parsed > 0)
        {
            return parsed;
        }

        errors["idSubCartera"] = ["idSubCartera debe ser un entero positivo."];
        return null;
    }

    private static string? Normalizar(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
