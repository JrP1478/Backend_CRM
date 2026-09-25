using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class CrmOpcionPermisoRepository(CrmDbContext context)
    : ICrmOpcionPermisoRepository
{
    public async Task<bool> TienePermisoAsync(
        int idUsuario,
        int? idGrupo,
        string codigoOpcion,
        CrmOptionPermission permiso,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 ||
            string.IsNullOrWhiteSpace(codigoOpcion) ||
            (idGrupo.HasValue && idGrupo.Value <= 0))
        {
            return false;
        }

        var normalizedOptionCode = codigoOpcion.Trim();
        var idOpcion = await context.Crm_Opcions
            .AsNoTracking()
            .Where(option =>
                option.sCodigoOpcion == normalizedOptionCode &&
                option.bEstado &&
                option.bVisible)
            .Select(option => (int?)option.nId_Opcion)
            .SingleOrDefaultAsync(cancellationToken);

        return idOpcion.HasValue &&
            await TienePermisoInternoAsync(
                idUsuario,
                idGrupo,
                idOpcion.Value,
                permiso,
                cancellationToken);
    }

    public async Task<bool> TienePermisoAsync(
        int idUsuario,
        int? idGrupo,
        int idOpcion,
        CrmOptionPermission permiso,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 ||
            idOpcion <= 0 ||
            (idGrupo.HasValue && idGrupo.Value <= 0))
        {
            return false;
        }

        var optionExists = await context.Crm_Opcions
            .AsNoTracking()
            .AnyAsync(option =>
                option.nId_Opcion == idOpcion &&
                option.bEstado &&
                option.bVisible,
                cancellationToken);

        return optionExists &&
            await TienePermisoInternoAsync(
                idUsuario,
                idGrupo,
                idOpcion,
                permiso,
                cancellationToken);
    }

    private async Task<bool> TienePermisoInternoAsync(
        int idUsuario,
        int? idGrupo,
        int idOpcion,
        CrmOptionPermission permiso,
        CancellationToken cancellationToken)
    {
        var user = await context.Crm_Usuarios
            .AsNoTracking()
            .Where(candidate =>
                candidate.nId_Usuario == idUsuario &&
                candidate.bEstado &&
                candidate.nid_perfil.HasValue &&
                candidate.nid_perfil.Value > 0)
            .Select(candidate => new
            {
                ProfileId = candidate.nid_perfil!.Value
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return false;
        }

        if (idGrupo.HasValue)
        {
            var currentGroupId = idGrupo.Value;
            var hasActiveMembership = await context.Crm_UGrupos
                .AsNoTracking()
                .AnyAsync(userGroup =>
                    userGroup.nId_Usuario == idUsuario &&
                    userGroup.nId_Grupo == currentGroupId &&
                    userGroup.bEstado == true &&
                    userGroup.bActivo == true &&
                    userGroup.Crm_Grupo.bEstado == true,
                    cancellationToken);

            if (!hasActiveMembership)
            {
                return false;
            }

            var userGroupPermissions = await context.Crm_UsuarioGrupoOpcions
                .AsNoTracking()
                .Where(assignment =>
                    assignment.nId_Usuario == idUsuario &&
                    assignment.nId_Grupo == currentGroupId &&
                    assignment.nId_Opcion == idOpcion &&
                    assignment.bEstado)
                .Select(assignment => new ProyeccionPermiso(
                    assignment.bConsultar,
                    assignment.bInsertar,
                    assignment.bEditar,
                    assignment.bEliminar,
                    assignment.bExportar))
                .Take(2)
                .ToArrayAsync(cancellationToken);

            // CRM trata un acceso especial activo como reemplazo completo del
            // permiso de perfil. Si hubiera más de uno, se deniega por inconsistencia.
            if (userGroupPermissions.Length > 1)
            {
                return false;
            }

            if (userGroupPermissions.Length == 1)
            {
                return TienePermiso(userGroupPermissions[0], permiso);
            }
        }

        var profilePermissions = await context.Crm_PerfilOpcions
            .AsNoTracking()
            .Where(assignment =>
                assignment.nId_Perfil == user.ProfileId &&
                assignment.nId_Opcion == idOpcion &&
                assignment.bEstado)
            .Select(assignment => new ProyeccionPermiso(
                assignment.bConsultar,
                assignment.bInsertar,
                assignment.bEditar,
                assignment.bEliminar,
                assignment.bExportar))
            .Take(2)
            .ToArrayAsync(cancellationToken);

        if (profilePermissions.Length != 1)
        {
            return false;
        }

        return TienePermiso(profilePermissions[0], permiso);
    }

    private static bool TienePermiso(
        ProyeccionPermiso permissions,
        CrmOptionPermission permiso) =>
        permiso switch
        {
            CrmOptionPermission.Consult => permissions.Consult == true,
            CrmOptionPermission.Insert => permissions.Insert == true,
            CrmOptionPermission.Edit => permissions.Edit == true,
            CrmOptionPermission.Delete => permissions.Delete == true,
            CrmOptionPermission.Export => permissions.Export == true,
            _ => false
        };

    private sealed record ProyeccionPermiso(
        bool? Consult,
        bool? Insert,
        bool? Edit,
        bool? Delete,
        bool? Export);
}
