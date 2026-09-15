using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IConfiguracionReporteClienteAnaliticaService
{
    Task<bool> RequiereSeleccionClienteAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyDictionary<int, bool>> RequiereSeleccionClienteMultipleAsync(
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AnaliticaConfiguracionReporteCliente>> ResolverAsync(
        int idOpcion,
        CancellationToken cancellationToken);
}
