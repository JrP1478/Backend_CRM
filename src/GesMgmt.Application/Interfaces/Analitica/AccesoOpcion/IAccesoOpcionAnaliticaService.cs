using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAccesoOpcionAnaliticaService
{
    Task<AnaliticaAccesoOpcionResult> ResolverAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<int, AnaliticaAccesoOpcionResult>> ResolverVariosAsync(
        int idUsuario,
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken);
}

public sealed record AnaliticaAccesoOpcionResult(
    bool Permitido,
    string ModoAlcance,
    IReadOnlyList<int> IdsGruposCoincidentes,
    IReadOnlyList<int> IdsGruposUsuarioActivos);
