using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AccesoAnaliticaService(
    ISisgesClienteUsuarioRepository users,
    IAnaliticaAlcanceClienteOpcionRepository scopes) : IAccesoAnaliticaService
{
    public async Task<IReadOnlyList<AnaliticaClientePermitido>> ObtenerClientesPermitidosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var authorizedClients = await scopes.ObtenerClientesAutorizadosAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        if (authorizedClients.Count == 0)
        {
            return [];
        }

        var userClients = await users.ObtenerIdsClientesActivosAsync(
            idUsuario,
            cancellationToken);
        var activeUserClientIds = NormalizarIdsClientes(userClients).ToHashSet();

        return authorizedClients
            .Where(client =>
                client.IdClienteCrm > 0 &&
                activeUserClientIds.Contains(client.IdClienteCrm))
            .GroupBy(client => client.IdClienteCrm)
            .Select(group =>
            {
                var idCliente = group.Key;
                var name = group
                    .Select(client => client.Nombre?.Trim())
                    .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))
                    ?? $"Cartera {idCliente}";

                return new AnaliticaClientePermitido(idCliente, name);
            })
            .OrderBy(client => client.IdClienteCrm)
            .ToArray();
    }

    public async Task<IReadOnlyList<int>> ObtenerIdsClientesPermitidosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken)
    {
        // Resolve the Analítica-side authorization and client scope first.
        // Centro de Control de Cartera inherits authorization from its active client
        // scope; options with explicit user assignment keep user_option_scope.
        // A denied/unconfigured option therefore avoids the SISGES read.
        var allowedClients = await scopes.ObtenerIdsClientesAutorizadosAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        if (allowedClients.Count == 0)
        {
            return [];
        }

        // SISGES remains the source of truth for the user's current client
        // assignments, so revocations continue to take effect immediately.
        var userClients = await users.ObtenerIdsClientesActivosAsync(
            idUsuario,
            cancellationToken);

        return IntersectarIdsClientes(userClients, allowedClients);
    }

    public async Task<bool> EstaClientePermitidoAsync(
        int idUsuario,
        int idOpcion,
        int idCliente,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 || idOpcion <= 0 || idCliente <= 0)
        {
            return false;
        }

        // The exact checks meta different databases and return at most one row.
        // Running them concurrently reduces authorization latency for the normal
        // Centro de Control de Cartera path without caching user permissions.
        var analyticsAuthorizationTask = scopes.EsClienteAutorizadoAsync(
            idUsuario,
            idOpcion,
            idCliente,
            cancellationToken);
        var sisgesAssignmentTask = users.EsClienteActivoAsync(
            idUsuario,
            idCliente,
            cancellationToken);

        await Task.WhenAll(analyticsAuthorizationTask, sisgesAssignmentTask);

        return await analyticsAuthorizationTask && await sisgesAssignmentTask;
    }

    public async Task<IReadOnlyList<int>> ObtenerIdsClientesAlcanceClienteAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var results = await ObtenerIdsClientesAlcanceClienteMultipleAsync(
            idUsuario,
            [idOpcion],
            cancellationToken);

        return results.TryGetValue(idOpcion, out var idsClientes)
            ? idsClientes
            : Array.Empty<int>();
    }

    public async Task<IReadOnlyDictionary<int, IReadOnlyList<int>>> ObtenerIdsClientesAlcanceClienteMultipleAsync(
        int idUsuario,
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken)
    {
        var normalizedOptionIds = idsOpciones
            .Where(idOpcion => idOpcion > 0)
            .Distinct()
            .OrderBy(idOpcion => idOpcion)
            .ToArray();

        if (normalizedOptionIds.Length == 0)
        {
            return new Dictionary<int, IReadOnlyList<int>>();
        }

        // These reads are independent and meta different authorization
        // sources. Execute them concurrently, but share the resulting SISGES
        // client snapshot only inside this request so revocations remain fresh
        // on the next request.
        var userClientsTask = users.ObtenerIdsClientesActivosAsync(
            idUsuario,
            cancellationToken);
        var optionScopesTask = scopes.ObtenerAlcancesActivosAsync(
            normalizedOptionIds,
            cancellationToken);

        await Task.WhenAll(userClientsTask, optionScopesTask);

        var activeUserClientSet = NormalizarIdsClientes(await userClientsTask)
            .ToHashSet();
        var requestedOptionIds = normalizedOptionIds.ToHashSet();
        var optionClientIds = (await optionScopesTask)
            .Where(scope => requestedOptionIds.Contains(scope.IdOpcion))
            .GroupBy(scope => scope.IdOpcion)
            .ToDictionary(
                group => group.Key,
                group => NormalizarIdsClientes(
                    group.Select(scope => scope.IdClienteCrm)));

        var results = new Dictionary<int, IReadOnlyList<int>>(
            normalizedOptionIds.Length);

        foreach (var idOpcion in normalizedOptionIds)
        {
            if (!optionClientIds.TryGetValue(idOpcion, out var allowedClientIds))
            {
                results[idOpcion] = Array.Empty<int>();
                continue;
            }

            results[idOpcion] = allowedClientIds
                .Where(activeUserClientSet.Contains)
                .ToArray();
        }

        return results;
    }

    private static IReadOnlyList<int> IntersectarIdsClientes(
        IReadOnlyList<int> userClients,
        IReadOnlyList<int> allowedClients) =>
        userClients
            .Intersect(allowedClients)
            .OrderBy(idCliente => idCliente)
            .ToArray();

    private static int[] NormalizarIdsClientes(IEnumerable<int> idsClientes) =>
        idsClientes
            .Where(idCliente => idCliente > 0)
            .Distinct()
            .OrderBy(idCliente => idCliente)
            .ToArray();
}
