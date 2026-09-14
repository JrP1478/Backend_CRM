using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AccesoGrupoAnaliticaService(
    ISisgesGrupoUsuarioRepository users,
    IAnaliticaAlcanceGrupoOpcionRepository scopes)
    : IAccesoGrupoAnaliticaService
{
    public async Task<IReadOnlyList<int>> ObtenerIdsGruposAlcanceGrupoAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var userGroupsTask = users.ObtenerIdsGruposActivosAsync(
            idUsuario,
            cancellationToken);
        var allowedGroupsTask = scopes.ObtenerIdsGruposAsync(
            idOpcion,
            cancellationToken);

        var groupSets = await Task.WhenAll(
            userGroupsTask,
            allowedGroupsTask);

        return groupSets[0]
            .Intersect(groupSets[1])
            .OrderBy(idGrupo => idGrupo)
            .ToArray();
    }
}
