using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;

namespace GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;

public interface IConsultaSesionesPowerBiRepository
{
    Task ExpirarPendientesAsync(CancellationToken cancellationToken);

    Task<PanelSesionesPowerBiDatos> ObtenerPanelAsync(
        SesionesPowerBiConsultaFiltro filtro,
        CancellationToken cancellationToken);

    Task<DetalleSesionPowerBiDatos?> ObtenerDetalleAsync(
        Guid idSesion,
        CancellationToken cancellationToken);
}
