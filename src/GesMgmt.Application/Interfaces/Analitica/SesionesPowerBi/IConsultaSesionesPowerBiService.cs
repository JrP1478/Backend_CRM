using GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;

namespace GesMgmt.Application.Interfaces.Analitica.SesionesPowerBi;

public interface IConsultaSesionesPowerBiService
{
    Task<ConsultaSesionesPowerBiOperacionResultado> ObtenerPanelAsync(
        int idUsuario,
        int? idGrupo,
        ConsultarPanelSesionesPowerBiRequest request,
        CancellationToken cancellationToken);

    Task<ConsultaDetalleSesionPowerBiOperacionResultado> ObtenerDetalleAsync(
        int idUsuario,
        int? idGrupo,
        Guid idSesion,
        CancellationToken cancellationToken);
}
