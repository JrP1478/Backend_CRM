using GesMgmt.Application.DTOs.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IConsultaOpcionesUsuarioAnaliticaService
{
    Task<IReadOnlyList<AnaliticaOpcionUsuarioResponse>> ObtenerAsync(
        int idUsuario,
        CancellationToken cancellationToken);
}
