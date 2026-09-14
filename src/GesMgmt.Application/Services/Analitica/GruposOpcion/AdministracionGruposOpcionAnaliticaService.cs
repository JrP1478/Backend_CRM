using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AdministracionGruposOpcionAnaliticaService(
    IAnaliticaOpcionConfiguracionRepository optionRepository,
    IAnaliticaAlcanceGrupoOpcionRepository repository)
    : IAdministracionGruposOpcionAnaliticaService
{
    public async Task<AnaliticaGruposOpcionResponse?> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (!await optionRepository.ExisteAsync(idOpcion, cancellationToken))
        {
            return null;
        }

        var idsGrupos = await repository.ObtenerIdsGruposAsync(
            idOpcion,
            cancellationToken);

        return new AnaliticaGruposOpcionResponse(idOpcion, idsGrupos);
    }

    public async Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyCollection<int>? requestedGroupIds,
        int idUsuario,
        CancellationToken cancellationToken)
    {
        if (!await optionRepository.ExisteAsync(idOpcion, cancellationToken))
        {
            return AnaliticaAdministracionComandoResult.NoEncontrado();
        }

        var idsGrupos = requestedGroupIds ?? [];

        if (idsGrupos.Any(idGrupo => idGrupo <= 0))
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                "Grupos inválidos",
                "Todos los idsGrupos deben ser enteros positivos.");
        }

        var newGroupIds = idsGrupos
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToArray();

        if (AnaliticaAlcanceGrupoOpcionReglas.RequiereExactamenteUnGrupo(idOpcion) &&
            newGroupIds.Length != 1)
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                "Grupo asociado inválido",
                "Gestión Integral de Cobranza debe tener exactamente un grupo SISGES asociado.");
        }

        var previousGroupIds = await repository.ObtenerIdsGruposAsync(
            idOpcion,
            cancellationToken);
        var normalizedPreviousGroupIds = previousGroupIds
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToArray();

        if (!normalizedPreviousGroupIds.SequenceEqual(newGroupIds))
        {
            await repository.ReemplazarAsync(
                idOpcion,
                normalizedPreviousGroupIds,
                newGroupIds,
                idUsuario,
                cancellationToken);
        }

        return AnaliticaAdministracionComandoResult.Exito();
    }
}
