namespace GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;

public sealed record AbrirSesionPowerBiResponse(
    Guid IdSesion,
    DateTime FechaInicioUtc);

public static class SesionPowerBiOperacionEstado
{
    public const string Exito = "EXITO";
    public const string SolicitudInvalida = "SOLICITUD_INVALIDA";
    public const string NoEncontrado = "NO_ENCONTRADO";
    public const string Denegado = "DENEGADO";
    public const string Finalizada = "FINALIZADA";
}

public sealed record SesionPowerBiOperacionResultado(
    string Estado,
    AbrirSesionPowerBiResponse? Apertura = null,
    string? Titulo = null,
    string? Detalle = null);
