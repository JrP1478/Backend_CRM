using GesMgmt.Domain.Constants;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface ICrmOpcionPermisoRepository
{
    Task<bool> TienePermisoAsync(
        int idUsuario,
        int? idGrupo,
        string codigoOpcion,
        CrmOptionPermission permiso,
        CancellationToken cancellationToken);

    Task<bool> TienePermisoAsync(
        int idUsuario,
        int? idGrupo,
        int idOpcion,
        CrmOptionPermission permiso,
        CancellationToken cancellationToken);
}
