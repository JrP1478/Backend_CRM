using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class ConfiguracionPowerBiAnaliticaService(
    IAnaliticaOpcionConfiguracionRepository optionRepository,
    IAnaliticaAlcanceGrupoOpcionRepository optionGroupRepository,
    ICrmGrupoClienteRepository clientGroupRepository,
    IConfiguracionReporteClienteAnaliticaService configurationService,
    ISeguridadPowerBiAnaliticaPolicy powerBiSecurityPolicy,
    IAnaliticaConfiguracionPowerBiWriter writer)
    : IConfiguracionPowerBiAnaliticaService
{
    public async Task<AnaliticaConfiguracionPowerBiResponse> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var existsTask = optionRepository.ExisteAsync(
            idOpcion,
            cancellationToken);
        var availableGroupsTask = clientGroupRepository.ObtenerTodosGruposActivosAsync(
            cancellationToken);

        await Task.WhenAll(existsTask, availableGroupsTask);

        var availableGroups = (await availableGroupsTask)
            .Where(group => group.IdGrupo > 0 && group.IdCliente > 0)
            .GroupBy(group => group.IdGrupo)
            .Select(group => group.First())
            .OrderBy(group => group.NombreGrupo, StringComparer.OrdinalIgnoreCase)
            .ThenBy(group => group.IdGrupo)
            .Select(group => new AnaliticaConfiguracionPowerBiGrupo(
                group.IdGrupo,
                group.IdCliente,
                group.NombreGrupo.Trim()))
            .ToArray();

        if (!await existsTask)
        {
            return new AnaliticaConfiguracionPowerBiResponse(
                idOpcion,
                false,
                Array.Empty<int>(),
                availableGroups,
                Array.Empty<AnaliticaIncrustacionReporteClienteOpcion>());
        }

        var groupIdsResult = await optionGroupRepository.ObtenerIdsGruposAsync(
            idOpcion,
            cancellationToken);
        var configurations = await configurationService.ResolverAsync(
            idOpcion,
            cancellationToken);

        var idsGrupos = groupIdsResult
            .Where(idGrupo => idGrupo > 0)
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToArray();
        var clientes = configurations
            .Select(AnaliticaConfiguracionReporteClienteContratoMapper.Map)
            .ToArray();

        return new AnaliticaConfiguracionPowerBiResponse(
            idOpcion,
            true,
            idsGrupos,
            availableGroups,
            clientes);
    }

    public async Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        ActualizarAnaliticaConfiguracionPowerBiRequest request,
        int idUsuario,
        CancellationToken cancellationToken)
    {
        var codigoOpcion = request.CodigoOpcion?.Trim();
        var nombreOpcion = request.NombreOpcion?.Trim();

        if (string.IsNullOrWhiteSpace(codigoOpcion) ||
            string.IsNullOrWhiteSpace(nombreOpcion))
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                "Datos de opción inválidos",
                "codigoOpcion y nombreOpcion son obligatorios.");
        }

        var requestedGroupIds = request.IdsGrupos ?? [];

        if (requestedGroupIds.Any(idGrupo => idGrupo <= 0))
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                "Grupos inválidos",
                "Todos los idsGrupos deben ser enteros positivos.");
        }

        var idsGrupos = requestedGroupIds
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToArray();

        if (AnaliticaAlcanceGrupoOpcionReglas.RequiereExactamenteUnGrupo(idOpcion) &&
            idsGrupos.Length != 1)
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                "Grupo asociado inválido",
                "Gestión Integral de Cobranza debe tener exactamente un grupo CRM asociado.");
        }

        var previousGroupIds = await optionGroupRepository.ObtenerIdsGruposAsync(
            idOpcion,
            cancellationToken);
        var requestedPublications = request.Publicaciones ?? [];
        IReadOnlyList<AnaliticaConfiguracionReporteCliente> configurations =
            requestedPublications.Count == 0
                ? Array.Empty<AnaliticaConfiguracionReporteCliente>()
                : await configurationService.ResolverAsync(
                    idOpcion,
                    cancellationToken);

        var publicationValidation = AnaliticaPublicacionReporteClienteActualizarValidator.Validar(
            request.Publicaciones,
            configurations,
            powerBiSecurityPolicy.PermitirPublicarEnWeb);

        if (publicationValidation.Error is not null)
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                publicationValidation.Error.Titulo,
                publicationValidation.Error.Detalle);
        }

        await writer.ActualizarAsync(
            idOpcion,
            codigoOpcion,
            nombreOpcion,
            request.EsActivo,
            previousGroupIds,
            idsGrupos,
            publicationValidation.Updates,
            idUsuario,
            cancellationToken);

        return AnaliticaAdministracionComandoResult.Exito();
    }
}
