using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class ConfiguracionReporteClienteAnaliticaService(
    IAnaliticaCatalogoReporteClienteRepository catalogRepository,
    IAnaliticaAlcanceReporteClienteRepository scopeRepository,
    IAnaliticaIncrustacionReporteClienteRepository embedRepository,
    ICrmGrupoClienteRepository clientGroupRepository,
    ISeguridadPowerBiAnaliticaPolicy powerBiSecurityPolicy)
    : IConfiguracionReporteClienteAnaliticaService
{
    public async Task<bool> RequiereSeleccionClienteAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (idOpcion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idOpcion));
        }

        if (catalogRepository.Soporta(idOpcion))
        {
            return true;
        }

        return await scopeRepository.TieneAlgunAlcanceAsync(
            idOpcion,
            cancellationToken);
    }

    public async Task<IReadOnlyDictionary<int, bool>> RequiereSeleccionClienteMultipleAsync(
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
            return new Dictionary<int, bool>();
        }

        var result = normalizedOptionIds
            .ToDictionary(
                idOpcion => idOpcion,
                catalogRepository.Soporta);
        var scopeBackedOptionIds = result
            .Where(pair => !pair.Value)
            .Select(pair => pair.Key)
            .ToArray();

        if (scopeBackedOptionIds.Length == 0)
        {
            return result;
        }

        var optionIdsWithActiveScope = await scopeRepository
            .ObtenerIdsOpcionesConAlcanceActivoAsync(
                scopeBackedOptionIds,
                cancellationToken);

        foreach (var idOpcion in optionIdsWithActiveScope)
        {
            if (result.ContainsKey(idOpcion))
            {
                result[idOpcion] = true;
            }
        }

        return result;
    }

    public async Task<IReadOnlyList<AnaliticaConfiguracionReporteCliente>> ResolverAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (idOpcion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idOpcion));
        }

        var usesLiveCatalog = catalogRepository.Soporta(idOpcion);

        IReadOnlyList<AnaliticaCatalogoReporteClienteItem> catalogItems =
            usesLiveCatalog
                ? await catalogRepository.ObtenerActualAsync(
                    idOpcion,
                    cancellationToken)
                : Array.Empty<AnaliticaCatalogoReporteClienteItem>();

        var scopeMappings = await scopeRepository.ObtenerMapeosAsync(
            idOpcion,
            cancellationToken);
        var publicaciones = await embedRepository.ObtenerActivasPorOpcionAsync(
            idOpcion,
            cancellationToken);

        var idsClientes = catalogItems
            .Select(item => item.IdClienteCrm)
            .Concat(scopeMappings.Select(mapping => mapping.IdClienteCrm))
            .Concat(publicaciones.Select(publication => publication.IdClienteCrm))
            .Where(idCliente => idCliente > 0)
            .Distinct()
            .ToArray();

        var clientGroups = await clientGroupRepository.ObtenerGruposActivosAsync(
            idsClientes,
            cancellationToken);

        return AnaliticaConfiguracionReporteClienteResolver.Resolver(
            usesLiveCatalog,
            catalogItems,
            scopeMappings,
            publicaciones,
            clientGroups,
            powerBiSecurityPolicy.PermitirPublicarEnWeb);
    }
}
