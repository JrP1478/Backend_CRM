namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

/// <resumen>
/// Shared Centro de Control de Cartera contract for selecting a single business-unit scope.
/// The canonical code is the value persisted in analitica.dim_cartera.unidad_negocio_origen;
/// the API does not invent a second business-unit identifier.
/// </resumen>
public static class UnidadNegocioCarteraContrato
{
    public const int MaxCodeLength = 150;

    public static bool IntentarNormalizarSolicitado(
        string? requestedBusinessUnit,
        out string? normalizedBusinessUnit,
        out Dictionary<string, string[]> errors)
    {
        errors = [];
        return IntentarNormalizar(
            "unidadNegocio",
            requestedBusinessUnit,
            out normalizedBusinessUnit,
            errors);
    }

    public static bool IntentarResolver(
        string? requestedBusinessUnit,
        string? backwardCompatibleDefaultBusinessUnit,
        IEnumerable<string?> sourceBusinessUnits,
        out UnidadNegocioCarteraSeleccion selection,
        out Dictionary<string, string[]> errors)
    {
        ArgumentNullException.ThrowIfNull(sourceBusinessUnits);

        errors = [];

        if (!IntentarNormalizar(
                "unidadNegocio",
                requestedBusinessUnit,
                out var normalizedRequested,
                errors)
            || !IntentarNormalizar(
                "defaultBusinessUnit",
                backwardCompatibleDefaultBusinessUnit,
                out var normalizedDefault,
                errors))
        {
            selection = UnidadNegocioCarteraSeleccion.Empty;
            return false;
        }

        var availableBusinessUnits = sourceBusinessUnits
            .Where(static value => !string.IsNullOrWhiteSpace(value))
            .Select(static value => value!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(static value => value, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (availableBusinessUnits.Any(
                static value => value.Length > MaxCodeLength
                    || value.Any(char.IsControl)))
        {
            errors["unidadNegocio"] =
            ["Analítica contiene una unidad de negocio con formato no válido."];
            selection = UnidadNegocioCarteraSeleccion.Empty;
            return false;
        }

        if (normalizedRequested is not null)
        {
            return IntentarSeleccionarCanonico(
                normalizedRequested,
                availableBusinessUnits,
                wasDefaulted: false,
                out selection,
                errors);
        }

        if (availableBusinessUnits.Length == 0)
        {
            selection = UnidadNegocioCarteraSeleccion.Empty;
            return true;
        }

        if (availableBusinessUnits.Length == 1)
        {
            selection = new UnidadNegocioCarteraSeleccion(
                availableBusinessUnits[0],
                availableBusinessUnits,
                WasDefaulted: true);
            return true;
        }

        if (normalizedDefault is not null)
        {
            return IntentarSeleccionarCanonico(
                normalizedDefault,
                availableBusinessUnits,
                wasDefaulted: true,
                out selection,
                errors);
        }

        errors["unidadNegocio"] =
        ["unidadNegocio es obligatorio cuando el cliente tiene múltiples unidad de negocios y no existe un default backward-compatible configurado."];
        selection = UnidadNegocioCarteraSeleccion.Empty;
        return false;
    }

    private static bool IntentarSeleccionarCanonico(
        string candidate,
        IReadOnlyList<string> availableBusinessUnits,
        bool wasDefaulted,
        out UnidadNegocioCarteraSeleccion selection,
        IDictionary<string, string[]> errors)
    {
        var canonical = availableBusinessUnits.FirstOrDefault(
            value => string.Equals(
                value,
                candidate,
                StringComparison.OrdinalIgnoreCase));

        if (canonical is null)
        {
            errors["unidadNegocio"] =
            ["unidadNegocio no está disponible para el cliente autorizado."];
            selection = UnidadNegocioCarteraSeleccion.Empty;
            return false;
        }

        selection = new UnidadNegocioCarteraSeleccion(
            canonical,
            availableBusinessUnits,
            wasDefaulted);
        return true;
    }

    private static bool IntentarNormalizar(
        string fieldName,
        string? value,
        out string? normalized,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            normalized = null;
            return true;
        }

        normalized = value.Trim();

        if (normalized.Length > MaxCodeLength)
        {
            errors[fieldName] =
            [$"{fieldName} no puede exceder {MaxCodeLength} caracteres."];
            return false;
        }

        if (normalized.Any(char.IsControl))
        {
            errors[fieldName] =
            [$"{fieldName} contiene caracteres no válidos."];
            return false;
        }

        return true;
    }
}

public sealed record UnidadNegocioCarteraSeleccion(
    string? SelectedBusinessUnit,
    IReadOnlyList<string> UnidadesNegocioDisponibles,
    bool WasDefaulted)
{
    public static UnidadNegocioCarteraSeleccion Empty { get; } =
        new(null, [], WasDefaulted: false);

    public bool TieneMultiplesUnidadesNegocio => UnidadesNegocioDisponibles.Count > 1;
}


/// <resumen>
/// Explicit compatibility rules that belong to the Centro de Control de Cartera API boundary.
/// They do not redefine the dimensional model: source_business_unit remains the canonical
/// business-unit identity.
/// </resumen>
public static class UnidadNegocioCarteraPolicy
{
    public const int ClaroCrmClientId = 95;
    public const string ClaroAdministrative = "CLARO ADMINISTRATIVO";
    public const string ClaroGovernment = "CLARO GOBIERNO";

    public static string? ObtenerPredeterminadoCompatible(int idClienteCrm) =>
        idClienteCrm == ClaroCrmClientId
            ? ClaroAdministrative
            : null;

    public static string? ResolverSolicitadoOPredeterminado(
        int idClienteCrm,
        string? requestedBusinessUnit) =>
        requestedBusinessUnit ?? ObtenerPredeterminadoCompatible(idClienteCrm);

    public static bool PuedeUsarMetaNivelCliente(
        int idClienteCrm,
        string? unidadNegocio)
    {
        _ = idClienteCrm;
        _ = unidadNegocio;

        // The monthly meta remains client/campana scoped. For CLARO, both
        // business units intentionally share that same meta amount/curve;
        // actual recovery is still restricted by UnidadNegocio downstream.
        return true;
    }
}
