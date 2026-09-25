namespace GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;

public sealed record SnapshotIdentidadReportePowerBi(
    int IdUsuario,
    string UsuarioLogin,
    string UsuarioNombre,
    int IdOpcionReporte,
    string ReporteNombre);
