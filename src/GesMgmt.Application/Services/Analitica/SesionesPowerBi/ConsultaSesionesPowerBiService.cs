using GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Interfaces.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Constants.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;

namespace GesMgmt.Application.Services.Analitica.SesionesPowerBi;

public sealed class ConsultaSesionesPowerBiService(
    ICrmOpcionPermisoRepository permisoRepository,
    IConfiguracionReporteClienteAnaliticaService configuracionReporteClienteService,
    IConsultaSesionesPowerBiRepository repository)
    : IConsultaSesionesPowerBiService
{
    private const int PaginaPredeterminada = 1;
    private const int TamanoPaginaPredeterminado = 25;
    private const int MaxTamanoPagina = 100;
    private const int MaxPagina = 100_000;
    private const int MaxDiasConsulta = 366;
    private const int MaxLongitudBusqueda = 120;

    private static readonly HashSet<string> EstadosPermitidos =
        new(StringComparer.Ordinal)
        {
            EstadoSesionPowerBi.Activa,
            EstadoSesionPowerBi.Pausada,
            EstadoSesionPowerBi.Cerrada,
            EstadoSesionPowerBi.Expirada
        };

    private static readonly HashSet<string> OrdenesPermitidos =
        new(StringComparer.Ordinal)
        {
            "inicio_desc",
            "tiempo_desc",
            "usuario_asc",
            "reporte_asc"
        };

    public async Task<ConsultaSesionesPowerBiOperacionResultado> ObtenerPanelAsync(
        int idUsuario,
        int? idGrupo,
        ConsultarPanelSesionesPowerBiRequest request,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0)
        {
            return Invalida("No se pudo identificar al usuario CRM.");
        }

        var permitido = await permisoRepository.TienePermisoAsync(
            idUsuario,
            idGrupo,
            CrmCodigosOpcion.SesionesBi,
            CrmOptionPermission.Consult,
            cancellationToken);

        if (!permitido)
        {
            return Denegado(
                "El usuario no tiene permiso Consultar sobre el módulo Sesiones BI.");
        }

        var ahora = DateTimeOffset.UtcNow;
        var hasta = request.HastaUtc ?? ahora;
        var desde = request.DesdeUtc ?? hasta.AddDays(-7);

        if (desde >= hasta)
        {
            return Invalida("DesdeUtc debe ser anterior a HastaUtc.");
        }

        if (hasta - desde > TimeSpan.FromDays(MaxDiasConsulta))
        {
            return Invalida(
                $"El período consultado no puede superar {MaxDiasConsulta} días.");
        }

        if (!IdValido(request.IdOpcionReporte) ||
            !IdValido(request.IdUsuario) ||
            !IdValido(request.IdCliente))
        {
            return Invalida(
                "Los identificadores de reporte, usuario y cliente deben ser enteros positivos.");
        }

        var estado = NormalizarEstado(request.Estado);
        if (!string.IsNullOrWhiteSpace(request.Estado) && estado is null)
        {
            return Invalida(
                $"Estado debe ser uno de: {string.Join(", ", EstadosPermitidos)}.");
        }

        var busqueda = request.Busqueda?.Trim();
        if (busqueda is { Length: > MaxLongitudBusqueda })
        {
            return Invalida(
                $"Busqueda no puede superar {MaxLongitudBusqueda} caracteres.");
        }

        if (busqueda?.Length == 0)
        {
            busqueda = null;
        }

        var pagina = request.Pagina ?? PaginaPredeterminada;
        var tamanoPagina = request.TamanoPagina ?? TamanoPaginaPredeterminado;

        if (pagina is <= 0 or > MaxPagina)
        {
            return Invalida($"Pagina debe estar entre 1 y {MaxPagina}.");
        }

        if (tamanoPagina is <= 0 or > MaxTamanoPagina)
        {
            return Invalida(
                $"TamanoPagina debe estar entre 1 y {MaxTamanoPagina}.");
        }

        var orden = NormalizarOrden(request.Orden);
        if (!string.IsNullOrWhiteSpace(request.Orden) && orden is null)
        {
            return Invalida(
                $"Orden debe ser uno de: {string.Join(", ", OrdenesPermitidos)}.");
        }

        orden ??= "inicio_desc";

        await repository.ExpirarPendientesAsync(cancellationToken);

        var datos = await repository.ObtenerPanelAsync(
            new SesionesPowerBiConsultaFiltro(
                desde.UtcDateTime,
                hasta.UtcDateTime,
                request.IdOpcionReporte,
                request.IdUsuario,
                request.IdCliente,
                estado,
                busqueda,
                orden,
                pagina,
                tamanoPagina),
            cancellationToken);

        var requiereSeleccionClientePorReporte =
            await configuracionReporteClienteService
                .RequiereSeleccionClienteMultipleAsync(
                    datos.Catalogos.Reportes
                        .Select(row => row.Id)
                        .ToArray(),
                    cancellationToken);

        return new ConsultaSesionesPowerBiOperacionResultado(
            ConsultaSesionesPowerBiOperacionEstado.Exito,
            Mapear(datos, requiereSeleccionClientePorReporte));
    }

    public async Task<ConsultaDetalleSesionPowerBiOperacionResultado> ObtenerDetalleAsync(
        int idUsuario,
        int? idGrupo,
        Guid idSesion,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0)
        {
            return DetalleInvalido("No se pudo identificar al usuario CRM.");
        }

        if (idSesion == Guid.Empty)
        {
            return DetalleInvalido("IdSesion no contiene un identificador válido.");
        }

        var permitido = await permisoRepository.TienePermisoAsync(
            idUsuario,
            idGrupo,
            CrmCodigosOpcion.SesionesBi,
            CrmOptionPermission.Consult,
            cancellationToken);

        if (!permitido)
        {
            return DetalleDenegado(
                "El usuario no tiene permiso Consultar sobre el módulo Sesiones BI.");
        }

        await repository.ExpirarPendientesAsync(cancellationToken);

        var datos = await repository.ObtenerDetalleAsync(
            idSesion,
            cancellationToken);

        if (datos is null)
        {
            return new ConsultaDetalleSesionPowerBiOperacionResultado(
                ConsultaDetalleSesionPowerBiOperacionEstado.NoEncontrado,
                Titulo: "Sesión BI no encontrada",
                Detalle: "La sesión solicitada no existe.");
        }

        var referenciaFin = datos.Sesion.FechaFinUtc
            ?? datos.Sesion.FechaUltimoHeartbeatUtc;
        var totalSegundosTranscurridos = Math.Clamp(
            (referenciaFin - datos.Sesion.FechaInicioUtc).TotalSeconds,
            0d,
            int.MaxValue);
        var segundosTranscurridos = (int)Math.Floor(totalSegundosTranscurridos);
        var segundosNoVisibles = Math.Max(
            0,
            segundosTranscurridos - datos.Sesion.SegundosVisibles);

        var response = new DetalleSesionPowerBiResponse(
            MapearSesion(datos.Sesion),
            segundosTranscurridos,
            segundosNoVisibles,
            datos.Eventos
                .Select(row => new EventoSesionPowerBiResponse(
                    row.IdEvento,
                    row.TipoEvento,
                    row.FechaEventoUtc,
                    row.SegundosVisibles,
                    row.Origen,
                    row.Detalle))
                .ToArray());

        return new ConsultaDetalleSesionPowerBiOperacionResultado(
            ConsultaDetalleSesionPowerBiOperacionEstado.Exito,
            response);
    }

    private static bool IdValido(int? id) =>
        !id.HasValue || id.Value > 0;

    private static string? NormalizarEstado(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            return null;
        }

        var normalized = estado.Trim().ToUpperInvariant();
        return EstadosPermitidos.Contains(normalized)
            ? normalized
            : null;
    }

    private static string? NormalizarOrden(string? orden)
    {
        if (string.IsNullOrWhiteSpace(orden))
        {
            return null;
        }

        var normalized = orden.Trim().ToLowerInvariant();
        return OrdenesPermitidos.Contains(normalized)
            ? normalized
            : null;
    }

    private static PanelSesionesPowerBiResponse Mapear(
        PanelSesionesPowerBiDatos datos,
        IReadOnlyDictionary<int, bool> requiereSeleccionClientePorReporte) =>
        new(
            datos.DesdeUtc,
            datos.HastaUtc,
            datos.GranularidadTendencia,
            new ResumenSesionesPowerBiResponse(
                datos.Resumen.SesionesActivas,
                datos.Resumen.TotalSesiones,
                datos.Resumen.UsuariosUnicos,
                datos.Resumen.SegundosVisibles,
                datos.Resumen.PromedioSegundosPorSesion),
            datos.UsoReportes
                .Select(row => new UsoReportePowerBiResponse(
                    row.IdOpcionReporte,
                    row.ReporteNombre,
                    row.Sesiones,
                    row.UsuariosUnicos,
                    row.SegundosVisibles))
                .ToArray(),
            datos.UsuariosMayorUso
                .Select(row => new UsoUsuarioPowerBiResponse(
                    row.IdUsuario,
                    row.UsuarioLogin,
                    row.UsuarioNombre,
                    row.Sesiones,
                    row.ReportesUnicos,
                    row.SegundosVisibles))
                .ToArray(),
            datos.Tendencia
                .Select(row => new TendenciaSesionesPowerBiResponse(
                    row.PeriodoUtc,
                    row.Sesiones,
                    row.UsuariosUnicos,
                    row.SegundosVisibles))
                .ToArray(),
            new CatalogosSesionesPowerBiResponse(
                datos.Catalogos.Reportes
                    .Select(row => new OpcionFiltroSesionPowerBiResponse(
                        row.Id,
                        row.Nombre,
                        requiereSeleccionClientePorReporte.GetValueOrDefault(row.Id)))
                    .ToArray(),
                datos.Catalogos.Usuarios
                    .Select(row => new OpcionFiltroSesionPowerBiResponse(
                        row.Id,
                        row.Nombre))
                    .ToArray(),
                datos.Catalogos.Clientes
                    .Select(row => new OpcionFiltroSesionPowerBiResponse(
                        row.Id,
                        row.Nombre))
                    .ToArray(),
                EstadosPermitidos.OrderBy(value => value).ToArray()),
            new PaginaSesionesPowerBiResponse(
                datos.Sesiones.Pagina,
                datos.Sesiones.TamanoPagina,
                datos.Sesiones.Total,
                datos.Sesiones.Items
                    .Select(MapearSesion)
                    .ToArray()));

    private static SesionPowerBiDetalleResponse MapearSesion(
        SesionPowerBiDetalleDatos row) =>
        new(
            row.IdSesion,
            row.IdUsuario,
            row.UsuarioLogin,
            row.UsuarioNombre,
            row.IdOpcionReporte,
            row.ReporteNombre,
            row.IdCliente,
            row.ClienteNombre,
            row.FechaInicioUtc,
            row.FechaUltimoHeartbeatUtc,
            row.FechaFinUtc,
            row.SegundosVisibles,
            row.EstaVisible,
            row.Estado,
            row.MotivoCierre);

    private static ConsultaDetalleSesionPowerBiOperacionResultado DetalleInvalido(
        string detalle) =>
        new(
            ConsultaDetalleSesionPowerBiOperacionEstado.SolicitudInvalida,
            Titulo: "Consulta de sesión BI inválida",
            Detalle: detalle);

    private static ConsultaDetalleSesionPowerBiOperacionResultado DetalleDenegado(
        string detalle) =>
        new(
            ConsultaDetalleSesionPowerBiOperacionEstado.Denegado,
            Titulo: "Acceso a Sesiones BI denegado",
            Detalle: detalle);

    private static ConsultaSesionesPowerBiOperacionResultado Invalida(
        string detalle) =>
        new(
            ConsultaSesionesPowerBiOperacionEstado.SolicitudInvalida,
            Titulo: "Consulta de Sesiones BI inválida",
            Detalle: detalle);

    private static ConsultaSesionesPowerBiOperacionResultado Denegado(
        string detalle) =>
        new(
            ConsultaSesionesPowerBiOperacionEstado.Denegado,
            Titulo: "Acceso a Sesiones BI denegado",
            Detalle: detalle);
}
