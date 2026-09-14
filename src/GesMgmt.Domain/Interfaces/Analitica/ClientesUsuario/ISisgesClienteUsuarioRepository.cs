using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface ISisgesClienteUsuarioRepository
{
    Task<IReadOnlyList<int>> ObtenerIdsClientesActivosAsync(
        int idUsuario,
        CancellationToken cancellationToken);

    Task<bool> EsClienteActivoAsync(
        int idUsuario,
        int idCliente,
        CancellationToken cancellationToken);
}
