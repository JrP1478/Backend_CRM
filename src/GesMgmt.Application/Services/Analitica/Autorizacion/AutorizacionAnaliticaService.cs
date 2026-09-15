using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AutorizacionAnaliticaService(
    ISisgesOpcionPermisoRepository permissionRepository)
    : IAutorizacionAnaliticaService
{
    public async Task<AnaliticaAutorizacionResult> PuedeAccederAdministracionAsync(
        int idUsuario,
        int? idGrupo,
        SisgesOptionPermission permiso,
        CancellationToken cancellationToken)
    {
        var allowed = await permissionRepository.TienePermisoAsync(
            idUsuario,
            idGrupo,
            SisgesCodigosOpcion.MantenerModulo,
            permiso,
            cancellationToken);

        if (allowed)
        {
            return AnaliticaAutorizacionResult.Permitir();
        }

        return AnaliticaAutorizacionResult.Denegar(
            $"El usuario no tiene permiso {ObtenerNombrePermiso(permiso)} sobre Mantener módulo.");
    }

    private static string ObtenerNombrePermiso(SisgesOptionPermission permiso) =>
        permiso switch
        {
            SisgesOptionPermission.Consult => "Consultar",
            SisgesOptionPermission.Insert => "Insertar",
            SisgesOptionPermission.Edit => "Editar",
            SisgesOptionPermission.Delete => "Eliminar",
            SisgesOptionPermission.Export => "Exportar",
            _ => "requerido"
        };
}
