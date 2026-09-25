using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface ICrmGrupoUsuarioRepository
{
    Task<IReadOnlyList<int>> ObtenerIdsGruposActivosAsync(
        int idUsuario,
        CancellationToken cancellationToken);
}
