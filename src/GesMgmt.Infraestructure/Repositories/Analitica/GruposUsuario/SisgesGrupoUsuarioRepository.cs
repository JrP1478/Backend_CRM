using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class SisgesGrupoUsuarioRepository(AvalDbContext context)
    : ISisgesGrupoUsuarioRepository
{
    public async Task<IReadOnlyList<int>> ObtenerIdsGruposActivosAsync(
        int idUsuario,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0)
        {
            return Array.Empty<int>();
        }

        return await context.av_UGrupos
            .AsNoTracking()
            .Where(userGroup =>
                userGroup.nId_Usuario == idUsuario &&
                userGroup.bEstado == true &&
                userGroup.bActivo == true &&
                userGroup.av_Grupo.bEstado == true &&
                userGroup.nId_Grupo.HasValue)
            .Select(userGroup => userGroup.nId_Grupo!.Value)
            .Distinct()
            .OrderBy(idGrupo => idGrupo)
            .ToListAsync(cancellationToken);
    }
}
