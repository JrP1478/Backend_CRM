namespace GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;

public sealed record AbrirSesionPowerBiRequest(
    int IdOpcion,
    int? IdCliente,
    string? ReportClient);

public sealed record ActualizarActividadSesionPowerBiRequest(
    int SegundosVisibles,
    bool Visible);

public sealed record CerrarSesionPowerBiRequest(
    int SegundosVisibles,
    string? MotivoCierre);
