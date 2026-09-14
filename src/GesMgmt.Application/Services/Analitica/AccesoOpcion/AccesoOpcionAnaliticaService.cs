using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AccesoOpcionAnaliticaService(
    ISisgesGrupoUsuarioRepository userGroups,
    IAnaliticaAlcanceGrupoOpcionRepository optionGroups,
    IAccesoAnaliticaService clientAccessService)
    : IAccesoOpcionAnaliticaService
{
    public async Task<AnaliticaAccesoOpcionResult> ResolverAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idUsuario));
        }

        if (idOpcion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idOpcion));
        }

        var results = await ResolverVariosAsync(
            idUsuario,
            [idOpcion],
            cancellationToken);

        return results[idOpcion];
    }

    public async Task<IReadOnlyDictionary<int, AnaliticaAccesoOpcionResult>> ResolverVariosAsync(
        int idUsuario,
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idUsuario));
        }

        var normalizedOptionIds = idsOpciones
            .Where(idOpcion => idOpcion > 0)
            .Distinct()
            .OrderBy(idOpcion => idOpcion)
            .ToArray();

        if (normalizedOptionIds.Length == 0)
        {
            return new Dictionary<int, AnaliticaAccesoOpcionResult>();
        }

        // Preserve the distinction between "never configured" and
        // "configured but currently inactive" by loading every scope row,
        // including inactive rows, in one Analítica round-trip.
        var configuredScopes = await optionGroups.ObtenerAlcancesAsync(
            normalizedOptionIds,
            cancellationToken);
        var requestedOptionIds = normalizedOptionIds.ToHashSet();
        var scopesByOption = configuredScopes
            .Where(scope => requestedOptionIds.Contains(scope.IdOpcion))
            .GroupBy(scope => scope.IdOpcion)
            .ToDictionary(group => group.Key, group => group.ToArray());

        // SISGES remains the source of truth. Read the user's active grupos
        // once for this resolution batch and share that request-scoped snapshot
        // across every requested option. Nothing is cached between requests.
        var activeUserGroupsTask = userGroups.ObtenerIdsGruposActivosAsync(
            idUsuario,
            cancellationToken);

        var legacyOptionIds = normalizedOptionIds
            .Where(idOpcion => !scopesByOption.ContainsKey(idOpcion))
            .ToArray();
        var legacyClientIdsTask = legacyOptionIds.Length == 0
            ? Task.FromResult<IReadOnlyDictionary<int, IReadOnlyList<int>>>(
                new Dictionary<int, IReadOnlyList<int>>())
            : clientAccessService.ObtenerIdsClientesAlcanceClienteMultipleAsync(
                idUsuario,
                legacyOptionIds,
                cancellationToken);

        await Task.WhenAll(activeUserGroupsTask, legacyClientIdsTask);

        var activeUserGroupIds = NormalizarIds(await activeUserGroupsTask);
        var legacyClientIdsByOption = await legacyClientIdsTask;
        var activeUserGroupSet = activeUserGroupIds.ToHashSet();
        var results = new Dictionary<int, AnaliticaAccesoOpcionResult>(
            normalizedOptionIds.Length);

        foreach (var idOpcion in normalizedOptionIds)
        {
            if (scopesByOption.TryGetValue(idOpcion, out var optionScopes))
            {
                var matchedGroupIds = NormalizarIds(
                        optionScopes
                            .Where(scope => scope.EsActivo)
                            .Select(scope => scope.IdGrupoSisges))
                    .Where(activeUserGroupSet.Contains)
                    .ToArray();

                results[idOpcion] = new AnaliticaAccesoOpcionResult(
                    matchedGroupIds.Length > 0,
                    "GROUP",
                    matchedGroupIds,
                    activeUserGroupIds);
                continue;
            }

            var legacyAllowed =
                legacyClientIdsByOption.TryGetValue(idOpcion, out var legacyClientIds) &&
                legacyClientIds.Count > 0;
            results[idOpcion] = new AnaliticaAccesoOpcionResult(
                legacyAllowed,
                "CLIENT_LEGACY",
                Array.Empty<int>(),
                activeUserGroupIds);
        }

        return results;
    }

    private static int[] NormalizarIds(IEnumerable<int> ids) =>
        ids
            .Where(id => id > 0)
            .Distinct()
            .OrderBy(id => id)
            .ToArray();
}
