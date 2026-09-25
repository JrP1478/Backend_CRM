using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record SeguimientoPromesasCarteraRequest(
    string? Campana,
    long? IdSubCartera,
    DateOnly FechaVencimiento,
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
        new(StringComparer.OrdinalIgnoreCase)
        {
            "pendiente",
            "parcial",
            "cumplida",
            "incumplida",
            "pagada-fuera-plazo"
        };

    private static readonly HashSet<string> ClavesOrdenPermitidas =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "idDeudor",
            "montoPromesa",
            "montoPagado",
            "montoPendiente",
            "etiquetaEstado",
            "fechaUltimoPago",
            "nombreAsesor",
            "nombreSupervisor"
        };

    public static bool IntentarCrear(
        string? campana,
        string? idSubCartera,
        string? fechaVencimiento,
        string? pagina,
        string? tamanoPagina,
        string? estado,
        string? ordenarPor,
        string? direccionOrden,
        out SeguimientoPromesasCarteraRequest? request,
        out Dictionary<string, string[]> errors)
    {
        errors = [];

        var campanaNormalizada = Normalizar(campana);
        if (campanaNormalizada is not null && !PatronCampana.IsMatch(campanaNormalizada))
        {
            errors["campana"] = ["campana debe tener formato YYYY-MM."];
        }

        var idSubCarteraParseado = ParsearIdSubCartera(idSubCartera, errors);
        var fechaVencimientoParseada = ParsearFechaVencimiento(fechaVencimiento, errors);
        PromesasCarteraPaginado.Parsear(
            pagina,
            tamanoPagina,
            errors,
            out var paginaParseada,
            out var tamanoPaginaParseado);

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

        if (errors.Count > 0 || !fechaVencimientoParseada.HasValue)
        {
            request = null;
            return false;
        }

        request = new SeguimientoPromesasCarteraRequest(
            campanaNormalizada,
            idSubCarteraParseado,
            fechaVencimientoParseada.Value,
            paginaParseada,
            tamanoPaginaParseado,
            estadoNormalizado?.ToLowerInvariant(),
            ordenarPorNormalizado,
            direccionOrdenNormalizada);
        return true;
    }

    private static DateOnly? ParsearFechaVencimiento(
        string? value,
        IDictionary<string, string[]> errors)
    {
        var normalized = Normalizar(value);
        if (normalized is null)
        {
            errors["fechaVencimiento"] = ["fechaVencimiento es obligatorio."];
            return null;
        }

        if (DateOnly.TryParseExact(
                normalized,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsed))
        {
            return parsed;
        }

        errors["fechaVencimiento"] = ["fechaVencimiento debe tener formato YYYY-MM-DD."];
        return null;
    }

    private static long? ParsearIdSubCartera(
        string? value,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

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
