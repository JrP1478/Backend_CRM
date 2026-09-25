using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record PromesasCarteraPaginacion(
    int Pagina,
    int TamanoPagina,
    long TotalItems,
    long TotalPages,
    bool TienePaginaAnterior,
    bool TienePaginaSiguiente)
{
    public static PromesasCarteraPaginacion Crear(
        int pagina,
        int tamanoPagina,
        long totalItems)
    {
        var totalPages = totalItems == 0
            ? 0
            : (totalItems / tamanoPagina) + (totalItems % tamanoPagina == 0 ? 0 : 1);

        return new PromesasCarteraPaginacion(
            pagina,
            tamanoPagina,
            totalItems,
            totalPages,
            totalPages > 0 && pagina > 1,
            pagina < totalPages);
    }
}
public static class PromesasCarteraPaginado
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 100;
    public const int MaxPageSize = 200;

    public static void Parsear(
        string? pagina,
        string? tamanoPagina,
        IDictionary<string, string[]> errors,
        out int paginaParseada,
        out int tamanoPaginaParseado)
    {
        paginaParseada = ParsearEnteroPositivo(
            pagina,
            DefaultPage,
            "pagina",
            "pagina debe ser un entero positivo.",
            errors);

        tamanoPaginaParseado = ParsearEnteroPositivo(
            tamanoPagina,
            DefaultPageSize,
            "tamanoPagina",
            $"tamanoPagina debe ser un entero entre 1 y {MaxPageSize}.",
            errors);

        if (tamanoPaginaParseado > MaxPageSize)
        {
            errors["tamanoPagina"] =
            [$"tamanoPagina debe ser un entero entre 1 y {MaxPageSize}."];
        }
    }

    private static int ParsearEnteroPositivo(
        string? value,
        int defaultValue,
        string field,
        string errorMessage,
        IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
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

        errors[field] = [errorMessage];
        return defaultValue;
    }
}
