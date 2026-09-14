using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaAlcanceGrupoOpcionRepository
{
    Task<bool> TieneAlgunAlcanceAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<int>> ObtenerIdsGruposAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AlcanceOpcionGrupoAnalitica>> ObtenerAlcancesAsync(
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken);

    Task ReemplazarAsync(
        int idOpcion,
        IReadOnlyCollection<int> previousGroupIds,
        IReadOnlyCollection<int> idsGrupos,
        int? idUsuario,
        CancellationToken cancellationToken);
}
