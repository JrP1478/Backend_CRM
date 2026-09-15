using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface ISisgesGrupoUsuarioRepository
{
    Task<IReadOnlyList<int>> ObtenerIdsGruposActivosAsync(
        int idUsuario,
        CancellationToken cancellationToken);
}
