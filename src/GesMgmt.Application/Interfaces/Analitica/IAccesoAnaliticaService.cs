using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAccesoAnaliticaService
{
    Task<IReadOnlyList<AnaliticaClientePermitido>> ObtenerClientesPermitidosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<int>> ObtenerIdsClientesPermitidosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);

    Task<bool> EstaClientePermitidoAsync(
        int idUsuario,
        int idOpcion,
        int idCliente,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<int>> ObtenerIdsClientesAlcanceClienteAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<int, IReadOnlyList<int>>> ObtenerIdsClientesAlcanceClienteMultipleAsync(
        int idUsuario,
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken);
}
