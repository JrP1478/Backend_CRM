namespace GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;

public sealed record ConsultarPanelSesionesPowerBiRequest(
    DateTimeOffset? DesdeUtc,
    DateTimeOffset? HastaUtc,
    int? IdOpcionReporte,
    int? IdUsuario,
    int? IdCliente,
    string? Estado,
    string? Busqueda,
    string? Orden,
    int? Pagina,
    int? TamanoPagina);

public sealed record ResumenSesionesPowerBiResponse(
    long SesionesActivas,
    long TotalSesiones,
    long UsuariosUnicos,
    long SegundosVisibles,
    int PromedioSegundosPorSesion);

public sealed record UsoReportePowerBiResponse(
    int IdOpcionReporte,
    string ReporteNombre,
    long Sesiones,
    long UsuariosUnicos,
    long SegundosVisibles);

public sealed record UsoUsuarioPowerBiResponse(
    int IdUsuario,
    string UsuarioLogin,
    string UsuarioNombre,
    long Sesiones,
    long ReportesUnicos,
    long SegundosVisibles);

public sealed record TendenciaSesionesPowerBiResponse(
    DateTime PeriodoUtc,
    long Sesiones,
    long UsuariosUnicos,
    long SegundosVisibles);

public sealed record OpcionFiltroSesionPowerBiResponse(
    int Id,
    string Nombre,
    bool RequiereSeleccionCliente = false);

public sealed record CatalogosSesionesPowerBiResponse(
    IReadOnlyList<OpcionFiltroSesionPowerBiResponse> Reportes,
    IReadOnlyList<OpcionFiltroSesionPowerBiResponse> Usuarios,
    IReadOnlyList<OpcionFiltroSesionPowerBiResponse> Clientes,
    IReadOnlyList<string> Estados);

public sealed record SesionPowerBiDetalleResponse(
    Guid IdSesion,
    int IdUsuario,
    string UsuarioLogin,
    string UsuarioNombre,
    int IdOpcionReporte,
    string ReporteNombre,
    int? IdCliente,
    string? ClienteNombre,
    DateTime FechaInicioUtc,
    DateTime FechaUltimoHeartbeatUtc,
    DateTime? FechaFinUtc,
    int SegundosVisibles,
    bool EstaVisible,
    string Estado,
    string? MotivoCierre);

public sealed record PaginaSesionesPowerBiResponse(
    int Pagina,
    int TamanoPagina,
    long Total,
    IReadOnlyList<SesionPowerBiDetalleResponse> Items);

public sealed record PanelSesionesPowerBiResponse(
    DateTime DesdeUtc,
    DateTime HastaUtc,
    string GranularidadTendencia,
    ResumenSesionesPowerBiResponse Resumen,
    IReadOnlyList<UsoReportePowerBiResponse> UsoReportes,
    IReadOnlyList<UsoUsuarioPowerBiResponse> UsuariosMayorUso,
    IReadOnlyList<TendenciaSesionesPowerBiResponse> Tendencia,
    CatalogosSesionesPowerBiResponse Catalogos,
    PaginaSesionesPowerBiResponse Sesiones);

public static class ConsultaSesionesPowerBiOperacionEstado
{
    public const string Exito = "EXITO";
    public const string SolicitudInvalida = "SOLICITUD_INVALIDA";
    public const string Denegado = "DENEGADO";
}

public sealed record ConsultaSesionesPowerBiOperacionResultado(
    string Estado,
    PanelSesionesPowerBiResponse? Panel = null,
    string? Titulo = null,
    string? Detalle = null);

public sealed record EventoSesionPowerBiResponse(
    long IdEvento,
    string TipoEvento,
    DateTime FechaEventoUtc,
    int SegundosVisibles,
    string Origen,
    string? Detalle);

public sealed record DetalleSesionPowerBiResponse(
    SesionPowerBiDetalleResponse Sesion,
    int SegundosTranscurridos,
    int SegundosNoVisiblesEstimados,
    IReadOnlyList<EventoSesionPowerBiResponse> Eventos);

public static class ConsultaDetalleSesionPowerBiOperacionEstado
{
    public const string Exito = "EXITO";
    public const string SolicitudInvalida = "SOLICITUD_INVALIDA";
    public const string NoEncontrado = "NO_ENCONTRADO";
    public const string Denegado = "DENEGADO";
}

public sealed record ConsultaDetalleSesionPowerBiOperacionResultado(
    string Estado,
    DetalleSesionPowerBiResponse? DetalleSesion = null,
    string? Titulo = null,
    string? Detalle = null);
