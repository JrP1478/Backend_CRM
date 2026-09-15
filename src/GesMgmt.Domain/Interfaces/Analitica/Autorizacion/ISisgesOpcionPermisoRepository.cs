using GesMgmt.Domain.Constants;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface ISisgesOpcionPermisoRepository
{
    Task<bool> TienePermisoAsync(
        int idUsuario,
        int? idGrupo,
        string codigoOpcion,
        SisgesOptionPermission permiso,
        CancellationToken cancellationToken);

    Task<bool> TienePermisoAsync(
        int idUsuario,
        int? idGrupo,
        int idOpcion,
        SisgesOptionPermission permiso,
        CancellationToken cancellationToken);
}
