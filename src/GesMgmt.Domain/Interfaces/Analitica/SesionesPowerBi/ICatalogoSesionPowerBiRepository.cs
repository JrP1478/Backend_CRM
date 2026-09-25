using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;

namespace GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;

public interface ICatalogoSesionPowerBiRepository
{
    Task<SnapshotIdentidadReportePowerBi?> ObtenerSnapshotAsync(
        int idUsuario,
        int idOpcionReporte,
        CancellationToken cancellationToken);
}
