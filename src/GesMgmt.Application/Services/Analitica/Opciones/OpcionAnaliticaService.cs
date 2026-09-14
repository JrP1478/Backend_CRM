using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class OpcionAnaliticaService(
    IAnaliticaOpcionConfiguracionRepository repository) : IOpcionAnaliticaService
{
    public async Task<IReadOnlyList<AnaliticaOpcionConfiguracionResponse>> ObtenerTodosAsync(
        CancellationToken cancellationToken)
    {
        var options = await repository.ObtenerTodosAsync(cancellationToken);

        return options
            .Select(option => new AnaliticaOpcionConfiguracionResponse(
                option.IdOpcion,
                option.CodigoOpcion,
                option.NombreOpcion))
            .ToArray();
    }

    public Task<bool> ExisteAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        repository.ExisteAsync(idOpcion, cancellationToken);

    public Task<bool> EstaActivoAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        repository.EstaActivoAsync(idOpcion, cancellationToken);

    public Task GuardarAsync(
        int idOpcion,
        string codigoOpcion,
        string nombreOpcion,
        bool esActivo,
        int? idUsuario,
        CancellationToken cancellationToken) =>
        repository.GuardarAsync(
            idOpcion,
            codigoOpcion,
            nombreOpcion,
            esActivo,
            idUsuario,
            cancellationToken);
}
