using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class ConsultaOpcionesUsuarioAnaliticaService(
    IAnaliticaOpcionUsuarioRepository repository) : IConsultaOpcionesUsuarioAnaliticaService
{
    public async Task<IReadOnlyList<AnaliticaOpcionUsuarioResponse>> ObtenerAsync(
        int idUsuario,
        CancellationToken cancellationToken)
    {
        var options = await repository.ObtenerOpcionesUsuarioAsync(
            idUsuario,
            cancellationToken);

        return options
            .Select(option => new AnaliticaOpcionUsuarioResponse(
                option.IdOpcion,
                option.CodigoOpcion,
                option.NombreOpcion))
            .ToArray();
    }
}
