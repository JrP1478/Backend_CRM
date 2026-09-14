namespace GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;

public sealed record SesionesPowerBiConsultaFiltro(
    DateTime DesdeUtc,
    DateTime HastaUtc,
    int? IdOpcionReporte,
    int? IdUsuario,
    int? IdCliente,
    string? Estado,
    string? Busqueda,
    string Orden,
    int Pagina,
    int TamanoPagina);

public sealed record ResumenSesionesPowerBiDatos(
    long SesionesActivas,
    long TotalSesiones,
    long UsuariosUnicos,
    long SegundosVisibles,
    int PromedioSegundosPorSesion);

public sealed record UsoReportePowerBiDatos(
    int IdOpcionReporte,
    string ReporteNombre,
    long Sesiones,
    long UsuariosUnicos,
    long SegundosVisibles);

public sealed record UsoUsuarioPowerBiDatos(
    int IdUsuario,
    string UsuarioLogin,
    string UsuarioNombre,
    long Sesiones,
    long ReportesUnicos,
    long SegundosVisibles);

public sealed record TendenciaSesionesPowerBiDatos(
    DateTime PeriodoUtc,
    long Sesiones,
    long UsuariosUnicos,
    long SegundosVisibles);

public sealed record OpcionFiltroSesionPowerBiDatos(
    int Id,
    string Nombre);

public sealed record CatalogosSesionesPowerBiDatos(
    IReadOnlyList<OpcionFiltroSesionPowerBiDatos> Reportes,
    IReadOnlyList<OpcionFiltroSesionPowerBiDatos> Usuarios,
    IReadOnlyList<OpcionFiltroSesionPowerBiDatos> Clientes);

public sealed record SesionPowerBiDetalleDatos(
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

public sealed record PaginaSesionesPowerBiDatos(
    int Pagina,
    int TamanoPagina,
    long Total,
    IReadOnlyList<SesionPowerBiDetalleDatos> Items);

public sealed record PanelSesionesPowerBiDatos(
    DateTime DesdeUtc,
    DateTime HastaUtc,
    string GranularidadTendencia,
    ResumenSesionesPowerBiDatos Resumen,
    IReadOnlyList<UsoReportePowerBiDatos> UsoReportes,
    IReadOnlyList<UsoUsuarioPowerBiDatos> UsuariosMayorUso,
    IReadOnlyList<TendenciaSesionesPowerBiDatos> Tendencia,
    CatalogosSesionesPowerBiDatos Catalogos,
    PaginaSesionesPowerBiDatos Sesiones);

public sealed record EventoSesionPowerBiDetalleDatos(
    long IdEvento,
    string TipoEvento,
    DateTime FechaEventoUtc,
    int SegundosVisibles,
    string Origen,
    string? Detalle);

public sealed record DetalleSesionPowerBiDatos(
    SesionPowerBiDetalleDatos Sesion,
    IReadOnlyList<EventoSesionPowerBiDetalleDatos> Eventos);
