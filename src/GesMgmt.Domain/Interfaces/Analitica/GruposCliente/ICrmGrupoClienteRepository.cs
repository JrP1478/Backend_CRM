using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface ICrmGrupoClienteRepository
{
    Task<IReadOnlyList<CrmGrupoCliente>> ObtenerTodosGruposActivosAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<CrmGrupoCliente>> ObtenerGruposActivosAsync(
        IReadOnlyCollection<int> idsClientes,
        CancellationToken cancellationToken);
}
