using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAdministracionGruposOpcionAnaliticaService
{
    Task<AnaliticaGruposOpcionResponse?> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyCollection<int>? requestedGroupIds,
        int idUsuario,
        CancellationToken cancellationToken);
}
