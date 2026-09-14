using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaOpcionConfiguracionRepository
{
    Task<bool> ExisteAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<bool> EstaActivoAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ConfiguracionOpcionAnalitica>> ObtenerTodosAsync(
        CancellationToken cancellationToken);

    Task GuardarAsync(
        int idOpcion,
        string codigoOpcion,
        string nombreOpcion,
        bool esActivo,
        int? idUsuario,
        CancellationToken cancellationToken);
}
