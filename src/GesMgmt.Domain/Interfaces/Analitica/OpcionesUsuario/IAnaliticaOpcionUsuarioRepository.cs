using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaOpcionUsuarioRepository
{
    Task<bool> TieneAccesoAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<OpcionUsuarioAnalitica>> ObtenerOpcionesUsuarioAsync(
        int idUsuario,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<int>> ObtenerIdsUsuariosAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task ReemplazarAsync(
        int idOpcion,
        IReadOnlyCollection<int> previousUserIds,
        IReadOnlyCollection<int> idsUsuarios,
        int? adminUserId,
        CancellationToken cancellationToken);
}
