using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AutorizacionAnaliticaService(
    ICrmOpcionPermisoRepository permissionRepository)
    : IAutorizacionAnaliticaService
{
    public async Task<AnaliticaAutorizacionResult> PuedeAccederAdministracionAsync(
        int idUsuario,
        int? idGrupo,
        CrmOptionPermission permiso,
        CancellationToken cancellationToken)
    {
        var allowed = await permissionRepository.TienePermisoAsync(
            idUsuario,
            idGrupo,
            CrmCodigosOpcion.MantenerModulo,
            permiso,
            cancellationToken);

        if (allowed)
        {
            return AnaliticaAutorizacionResult.Permitir();
        }

        return AnaliticaAutorizacionResult.Denegar(
            $"El usuario no tiene permiso {ObtenerNombrePermiso(permiso)} sobre Mantener módulo.");
    }

    private static string ObtenerNombrePermiso(CrmOptionPermission permiso) =>
        permiso switch
        {
            CrmOptionPermission.Consult => "Consultar",
            CrmOptionPermission.Insert => "Insertar",
            CrmOptionPermission.Edit => "Editar",
            CrmOptionPermission.Delete => "Eliminar",
            CrmOptionPermission.Export => "Exportar",
            _ => "requerido"
        };
}
