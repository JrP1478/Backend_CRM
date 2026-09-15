using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public static class AnaliticaConfiguracionReporteClienteResolver
{
    public static IReadOnlyList<AnaliticaConfiguracionReporteCliente> Resolver(
        bool usesLiveCatalog,
        IReadOnlyList<AnaliticaCatalogoReporteClienteItem> catalogItems,
        IReadOnlyList<AnaliticaAlcanceReporteClienteMapeo> scopeMappings,
        IReadOnlyList<AnaliticaIncrustacionReporteClienteAdministracionMapeo> publicaciones,
        IReadOnlyList<SisgesGrupoCliente> clientGroups,
        bool allowPublishToWeb = true)
    {
        var liveKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var scopeKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var values = new Dictionary<string, (int IdCliente, string Nombre)>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var item in catalogItems)
        {
            AgregarValor(
                liveKeys,
                values,
                item.IdClienteCrm,
                item.ClienteReporte);
        }

        foreach (var mapping in scopeMappings)
        {
            AgregarValor(
                scopeKeys,
                values,
                mapping.IdClienteCrm,
                mapping.ClienteReporte);
        }

        foreach (var publication in publicaciones)
        {
            AgregarValor(
                null,
                values,
                publication.IdClienteCrm,
                publication.ClienteReporte);
        }

        var candidateGroupsByClient = clientGroups
            .Where(group => group.IdGrupo > 0 && group.IdCliente > 0)
            .GroupBy(group => group.IdCliente)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<AnaliticaConfiguracionReporteClienteGrupo>)group
                    .GroupBy(candidate => candidate.IdGrupo)
                    .Select(candidateGroup => candidateGroup.First())
                    .Select(candidate => new AnaliticaConfiguracionReporteClienteGrupo(
                        candidate.IdGrupo,
                        NormalizarNombreGrupo(candidate.NombreGrupo, candidate.IdGrupo)))
                    .OrderBy(candidate => candidate.Nombre, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(candidate => candidate.IdGrupo)
                    .ToArray());

        var explicitGroupsByKey = scopeMappings
            .Where(mapping =>
                mapping.IdClienteCrm > 0 &&
                mapping.IdGrupoSisges > 0 &&
                !string.IsNullOrWhiteSpace(mapping.ClienteReporte))
            .GroupBy(
                mapping => ConstruirClave(mapping.IdClienteCrm, mapping.ClienteReporte),
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<int>)group
                    .Select(mapping => mapping.IdGrupoSisges)
                    .Distinct()
                    .OrderBy(idGrupo => idGrupo)
                    .ToArray(),
                StringComparer.OrdinalIgnoreCase);

        var publicationByKey = publicaciones
            .Where(publication =>
                publication.IdClienteCrm > 0 &&
                !string.IsNullOrWhiteSpace(publication.ClienteReporte) &&
                !string.IsNullOrWhiteSpace(publication.UrlIncrustacion))
            .GroupBy(
                publication => ConstruirClave(
                    publication.IdClienteCrm,
                    publication.ClienteReporte),
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.First().UrlIncrustacion?.Trim(),
                StringComparer.OrdinalIgnoreCase);

        var result = new List<AnaliticaConfiguracionReporteCliente>(values.Count);

        foreach (var (key, value) in values)
        {
            var estaDisponible = usesLiveCatalog
                ? liveKeys.Contains(key)
                : scopeKeys.Contains(key) || publicationByKey.ContainsKey(key);

            candidateGroupsByClient.TryGetValue(
                value.IdCliente,
                out var candidateGroups);

            candidateGroups ??= Array.Empty<AnaliticaConfiguracionReporteClienteGrupo>();

            explicitGroupsByKey.TryGetValue(
                key,
                out var explicitGroupIds);

            explicitGroupIds ??= Array.Empty<int>();

            var hasExplicitGroupConfiguration = explicitGroupIds.Count > 0;
            var candidateGroupIds = candidateGroups
                .Select(group => group.IdGrupo)
                .ToHashSet();

            string groupResolution;
            IReadOnlyList<int> effectiveGroupIds;

            if (!estaDisponible)
            {
                groupResolution = AnaliticaReporteClienteGrupoResolucion.NoDisponible;
                effectiveGroupIds = explicitGroupIds;
            }
            else if (hasExplicitGroupConfiguration)
            {
                var configuredGroupsAreValid = explicitGroupIds
                    .All(candidateGroupIds.Contains);

                groupResolution = configuredGroupsAreValid
                    ? AnaliticaReporteClienteGrupoResolucion.Configurado
                    : AnaliticaReporteClienteGrupoResolucion.ConfiguracionInvalida;

                effectiveGroupIds = configuredGroupsAreValid
                    ? explicitGroupIds
                    : Array.Empty<int>();
            }
            else if (candidateGroups.Count == 1)
            {
                groupResolution = AnaliticaReporteClienteGrupoResolucion.DetectadoAutomaticamente;
                effectiveGroupIds = [candidateGroups[0].IdGrupo];
            }
            else if (candidateGroups.Count == 0)
            {
                groupResolution = AnaliticaReporteClienteGrupoResolucion.Faltante;
                effectiveGroupIds = Array.Empty<int>();
            }
            else
            {
                groupResolution = AnaliticaReporteClienteGrupoResolucion.Ambiguo;
                effectiveGroupIds = Array.Empty<int>();
            }

            publicationByKey.TryGetValue(key, out var urlIncrustacion);

            var hasValidPublication =
                allowPublishToWeb &&
                UrlPublicacionWebPowerBi.IntentarNormalizar(
                    urlIncrustacion,
                    out _);

            var estaLista =
                estaDisponible &&
                AnaliticaReporteClienteGrupoResolucion.EstaResuelto(groupResolution) &&
                hasValidPublication;

            result.Add(
                new AnaliticaConfiguracionReporteCliente(
                    value.IdCliente,
                    value.Nombre,
                    estaDisponible,
                    groupResolution,
                    hasExplicitGroupConfiguration,
                    effectiveGroupIds,
                    candidateGroups,
                    urlIncrustacion,
                    estaLista));
        }

        return result
            .OrderByDescending(item => item.EstaDisponible)
            .ThenBy(item => item.Nombre, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.IdCliente)
            .ToArray();
    }

    private static void AgregarValor(
        HashSet<string>? meta,
        IDictionary<string, (int IdCliente, string Nombre)> values,
        int idCliente,
        string? name)
    {
        var normalizedName = name?.Trim() ?? string.Empty;

        if (idCliente <= 0 || normalizedName.Length == 0)
        {
            return;
        }

        var key = ConstruirClave(idCliente, normalizedName);
        meta?.Add(key);

        if (!values.ContainsKey(key))
        {
            values[key] = (idCliente, normalizedName);
        }
    }

    public static string ConstruirClave(int idCliente, string name) =>
        $"{idCliente}:{name.Trim()}";

    private static string NormalizarNombreGrupo(string? value, int idGrupo)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized)
            ? $"Grupo {idGrupo}"
            : normalized;
    }
}
