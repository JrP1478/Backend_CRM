using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaAlcanceClienteOpcionRepository
{
    Task<IReadOnlyList<int>> ObtenerIdsClientesAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<int>> ObtenerIdsClientesAutorizadosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AnaliticaAlcanceClienteAutorizadoEntrada>> ObtenerClientesAutorizadosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);

    Task<bool> EsClienteAutorizadoAsync(
        int idUsuario,
        int idOpcion,
        int idCliente,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AlcanceOpcionClienteAnalitica>> ObtenerAlcancesActivosAsync(
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken);

    Task ReemplazarAsync(
        int idOpcion,
        IReadOnlyCollection<int> previousClientIds,
        IReadOnlyCollection<int> idsClientes,
        int? idUsuario,
        CancellationToken cancellationToken);
}
