using GesMgmt.Application.DTOs.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IOpcionAnaliticaService
{
    Task<IReadOnlyList<AnaliticaOpcionConfiguracionResponse>> ObtenerTodosAsync(
        CancellationToken cancellationToken);

    Task<bool> ExisteAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<bool> EstaActivoAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task GuardarAsync(
        int idOpcion,
        string codigoOpcion,
        string nombreOpcion,
        bool esActivo,
        int? idUsuario,
        CancellationToken cancellationToken);
}
