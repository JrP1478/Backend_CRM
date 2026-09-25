using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAdministracionClientesOpcionAnaliticaService
{
    Task<AnaliticaClientesOpcionResponse?> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyCollection<int>? requestedClientIds,
        int idUsuario,
        CancellationToken cancellationToken);
}
