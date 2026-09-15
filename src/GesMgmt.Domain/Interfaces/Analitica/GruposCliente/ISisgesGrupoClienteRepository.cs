using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface ISisgesGrupoClienteRepository
{
    Task<IReadOnlyList<SisgesGrupoCliente>> ObtenerTodosGruposActivosAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<SisgesGrupoCliente>> ObtenerGruposActivosAsync(
        IReadOnlyCollection<int> idsClientes,
        CancellationToken cancellationToken);
}
