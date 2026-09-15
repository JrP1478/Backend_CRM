using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaAlcanceReporteClienteRepository
{
    Task<bool> TieneAlgunAlcanceAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AnaliticaAlcanceReporteClienteMapeo>> ObtenerMapeosAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<int>> ObtenerIdsOpcionesConAlcanceActivoAsync(
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken);

    Task ReemplazarParaClienteAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        IReadOnlyCollection<int> idsGrupos,
        int actualizadoPor,
        CancellationToken cancellationToken);

}
