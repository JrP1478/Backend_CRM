using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAdministracionOpcionesUsuarioAnaliticaService
{
    Task<AnaliticaUsuariosOpcionResponse?> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyCollection<int>? requestedUserIds,
        int adminUserId,
        CancellationToken cancellationToken);
}
