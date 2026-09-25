using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AccesoUsuarioPowerBiAnaliticaService(
    IAnaliticaOpcionConfiguracionRepository optionRepository,
    IAccesoOpcionAnaliticaService optionAccessService,
    IConfiguracionReporteClienteAnaliticaService reportClientConfigurationService)
    : IAccesoUsuarioPowerBiAnaliticaService
{
    public async Task<IReadOnlyList<AnaliticaPowerBiAccesoOpcion>> ResolverAsync(
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
            return Array.Empty<AnaliticaPowerBiAccesoOpcion>();
        }

        var activeOptions = await optionRepository.ObtenerTodosAsync(cancellationToken);
        var activeOptionIds = activeOptions
            .Select(option => option.IdOpcion)
            .ToHashSet();
        var requestedActiveOptionIds = normalizedOptionIds
            .Where(activeOptionIds.Contains)
            .ToArray();

        IReadOnlyDictionary<int, AnaliticaAccesoOpcionResult> accessByOption =
            requestedActiveOptionIds.Length == 0
                ? new Dictionary<int, AnaliticaAccesoOpcionResult>()
                : await optionAccessService.ResolverVariosAsync(
                    idUsuario,
                    requestedActiveOptionIds,
                    cancellationToken);

        var allowedOptionIds = requestedActiveOptionIds
            .Where(idOpcion =>
                accessByOption.TryGetValue(idOpcion, out var access) &&
                access.Permitido)
            .ToArray();
        IReadOnlyDictionary<int, bool> requiresClientSelectionByOption =
            allowedOptionIds.Length == 0
                ? new Dictionary<int, bool>()
                : await reportClientConfigurationService
                    .RequiereSeleccionClienteMultipleAsync(
                        allowedOptionIds,
                        cancellationToken);

        return normalizedOptionIds
            .Select(idOpcion => ResolverOpcion(
                idOpcion,
                activeOptionIds.Contains(idOpcion),
                accessByOption,
                requiresClientSelectionByOption))
            .ToArray();
    }

    private static AnaliticaPowerBiAccesoOpcion ResolverOpcion(
        int idOpcion,
        bool esActivo,
        IReadOnlyDictionary<int, AnaliticaAccesoOpcionResult> accessByOption,
        IReadOnlyDictionary<int, bool> requiresClientSelectionByOption)
    {
        if (!esActivo ||
            !accessByOption.TryGetValue(idOpcion, out var access) ||
            !access.Permitido ||
            !requiresClientSelectionByOption.TryGetValue(
                idOpcion,
                out var requiresClientSelection))
        {
            return new AnaliticaPowerBiAccesoOpcion(
                idOpcion,
                Permitido: false,
                RequiereSeleccionCliente: false);
        }

        return new AnaliticaPowerBiAccesoOpcion(
            idOpcion,
            Permitido: true,
            RequiereSeleccionCliente: requiresClientSelection);
    }
}
