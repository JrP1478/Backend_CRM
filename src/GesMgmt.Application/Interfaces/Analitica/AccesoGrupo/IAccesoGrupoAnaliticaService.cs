using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAccesoGrupoAnaliticaService
{
    Task<IReadOnlyList<int>> ObtenerIdsGruposAlcanceGrupoAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);
}
