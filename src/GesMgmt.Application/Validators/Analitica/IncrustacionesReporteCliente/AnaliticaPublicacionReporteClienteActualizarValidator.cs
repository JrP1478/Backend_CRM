using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Services.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Validators.Analitica;

public static class AnaliticaPublicacionReporteClienteActualizarValidator
{
    private const int LongitudMaximaValorClienteReporte = 150;
    private const int LongitudMaximaUrlIncrustacion = 2048;

    public static AnaliticaPublicacionReporteClienteValidacionResult Validar(
        IReadOnlyList<ActualizarAnaliticaIncrustacionReporteClienteOpcion>? publicaciones,
        IReadOnlyList<AnaliticaConfiguracionReporteCliente> configurations,
        bool allowPublishToWeb = true)
    {
        var configurationByKey = configurations.ToDictionary(
            configuration => ConstruirClave(
                configuration.IdCliente,
                configuration.Nombre),
            StringComparer.OrdinalIgnoreCase);

        var requestedPublications = publicaciones ?? [];
        var seenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var normalizedUpdates = new List<AnaliticaPublicacionReporteClienteActualizar>(
            requestedPublications.Count);

        foreach (var publication in requestedPublications)
        {
            var name = publication.Nombre?.Trim() ?? string.Empty;

            if (
                publication.IdCliente <= 0 ||
                name.Length == 0 ||
                name.Length > LongitudMaximaValorClienteReporte)
            {
                return Invalido(
                    "Configuración por cartera inválida",
                    "Cada configuración debe indicar un idCliente positivo y una cartera válida.");
            }

            var key = ConstruirClave(publication.IdCliente, name);

            if (!seenKeys.Add(key))
            {
                return Invalido(
                    "Configuración por cartera duplicada",
                    $"La cartera '{name}' fue enviada más de una vez.");
            }

            if (!configurationByKey.TryGetValue(key, out var configuration))
            {
                return Invalido(
                    "Cartera desconocida",
                    $"La cartera '{name}' no pertenece al catálogo ni a la configuración de esta opción Analítica.");
            }

            if (!configuration.EstaDisponible)
            {
                return Invalido(
                    "Cartera no disponible",
                    $"La cartera '{configuration.Nombre}' ya no está disponible actualmente en la fuente del reporte.");
            }

            var candidateGroupIds = configuration.GruposCandidatos
                .Select(group => group.IdGrupo)
                .ToHashSet();

            int[]? requestedGroupIds = null;

            if (publication.IdsGrupos is not null)
            {
                if (publication.IdsGrupos.Any(idGrupo => idGrupo <= 0))
                {
                    return Invalido(
                        "Grupo de cartera inválido",
                        $"Todos los grupos seleccionados para '{configuration.Nombre}' deben ser enteros positivos.");
                }

                requestedGroupIds = publication.IdsGrupos
                    .Distinct()
                    .OrderBy(idGrupo => idGrupo)
                    .ToArray();

                if (requestedGroupIds.Any(idGrupo => !candidateGroupIds.Contains(idGrupo)))
                {
                    return Invalido(
                        "Grupo fuera del cliente",
                        $"Uno o más grupos seleccionados para '{configuration.Nombre}' no están activos o no pertenecen al cliente SISGES {configuration.IdCliente}.");
                }
            }

            var effectiveGroupIds = requestedGroupIds ?? configuration.IdsGrupos;
            var rawUrl = publication.UrlIncrustacion?.Trim() ?? string.Empty;
            string? normalizedUrl = null;

            if (rawUrl.Length > 0)
            {
                if (!allowPublishToWeb)
                {
                    return Invalido(
                        "Publicar en web deshabilitado",
                        "La API no permite guardar publicaciones públicas de Power BI en este entorno.");
                }

                if (
                    rawUrl.Length > LongitudMaximaUrlIncrustacion ||
                    !UrlPublicacionWebPowerBi.IntentarNormalizar(rawUrl, out var parsedUrl))
                {
                    return Invalido(
                        "URL de publicación web inválida",
                        $"La URL configurada para '{configuration.Nombre}' debe usar https://app.powerbi.com/view?r=...");
                }

                normalizedUrl = parsedUrl;
            }

            if (normalizedUrl is not null && effectiveGroupIds.Count == 0)
            {
                return Invalido(
                    "Acceso de cartera pendiente",
                    $"Seleccione al menos un grupo SISGES para '{configuration.Nombre}' antes de habilitar su publicación.");
            }

            normalizedUpdates.Add(
                new AnaliticaPublicacionReporteClienteActualizar(
                    configuration.IdCliente,
                    configuration.Nombre,
                    requestedGroupIds,
                    normalizedUrl));
        }

        return new AnaliticaPublicacionReporteClienteValidacionResult(
            normalizedUpdates,
            null);
    }

    private static AnaliticaPublicacionReporteClienteValidacionResult Invalido(
        string title,
        string detail) =>
        new(
            Array.Empty<AnaliticaPublicacionReporteClienteActualizar>(),
            new AnaliticaPublicacionReporteClienteValidacionError(
                title,
                detail));

    private static string ConstruirClave(int idCliente, string name) =>
        AnaliticaConfiguracionReporteClienteResolver.ConstruirClave(idCliente, name);
}

public sealed record AnaliticaPublicacionReporteClienteValidacionResult(
    IReadOnlyList<AnaliticaPublicacionReporteClienteActualizar> Updates,
    AnaliticaPublicacionReporteClienteValidacionError? Error);

public sealed record AnaliticaPublicacionReporteClienteValidacionError(
    string Titulo,
    string Detalle);
