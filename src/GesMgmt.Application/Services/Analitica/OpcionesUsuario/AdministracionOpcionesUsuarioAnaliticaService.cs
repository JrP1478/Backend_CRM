using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AdministracionOpcionesUsuarioAnaliticaService(
    IAnaliticaOpcionUsuarioRepository repository,
    IAnaliticaOpcionConfiguracionRepository optionRepository)
    : IAdministracionOpcionesUsuarioAnaliticaService
{
    public async Task<AnaliticaUsuariosOpcionResponse?> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (!await optionRepository.ExisteAsync(idOpcion, cancellationToken))
        {
            return null;
        }

        var idsUsuarios = await repository.ObtenerIdsUsuariosAsync(
            idOpcion,
            cancellationToken);

        return new AnaliticaUsuariosOpcionResponse(idOpcion, idsUsuarios);
    }

    public async Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyCollection<int>? requestedUserIds,
        int adminUserId,
        CancellationToken cancellationToken)
    {
        if (!await optionRepository.ExisteAsync(idOpcion, cancellationToken))
        {
            return AnaliticaAdministracionComandoResult.NoEncontrado();
        }

        var idsUsuarios = requestedUserIds ?? [];

        if (idsUsuarios.Any(idUsuario => idUsuario <= 0))
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                "Usuarios inválidos",
                "Todos los idsUsuarios deben ser enteros positivos.");
        }

        var newUserIds = idsUsuarios
            .Distinct()
            .OrderBy(idUsuario => idUsuario)
            .ToArray();
        var previousUserIds = await repository.ObtenerIdsUsuariosAsync(
            idOpcion,
            cancellationToken);
        var normalizedPreviousUserIds = previousUserIds
            .Distinct()
            .OrderBy(idUsuario => idUsuario)
            .ToArray();

        if (!normalizedPreviousUserIds.SequenceEqual(newUserIds))
        {
            await repository.ReemplazarAsync(
                idOpcion,
                normalizedPreviousUserIds,
                newUserIds,
                adminUserId,
                cancellationToken);
        }

        return AnaliticaAdministracionComandoResult.Exito();
    }
}
