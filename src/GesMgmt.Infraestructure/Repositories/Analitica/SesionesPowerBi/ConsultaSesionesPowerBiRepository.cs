using GesMgmt.Domain.Constants.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.SesionesPowerBi;

internal sealed class ConsultaSesionesPowerBiRepository(AnaliticaDbContext context)
    : IConsultaSesionesPowerBiRepository
{
    private const int MaximoRanking = 10;

    public Task ExpirarPendientesAsync(CancellationToken cancellationToken) =>
        context.Database.ExecuteSqlRawAsync(
            "EXEC telemetria_analitica.sp_expirar_sesiones_power_bi;",
            cancellationToken);

    public async Task<PanelSesionesPowerBiDatos> ObtenerPanelAsync(
        SesionesPowerBiConsultaFiltro filtro,
        CancellationToken cancellationToken)
    {
        var consultaPeriodo = context.SesionesPowerBiAnalitica
            .AsNoTracking()
            .Where(row =>
                row.FechaInicioUtc >= filtro.DesdeUtc &&
                row.FechaInicioUtc < filtro.HastaUtc);

        var catalogos = await ObtenerCatalogosAsync(
            consultaPeriodo,
            filtro.IdOpcionReporte,
            cancellationToken);

        var consulta = AplicarFiltros(consultaPeriodo, filtro);

        var resumen = await ObtenerResumenAsync(
            consulta,
            cancellationToken);

        var usoReportes = await consulta
            .GroupBy(row => new
            {
                row.IdOpcionReporte,
                row.ReporteNombre
            })
            .Select(group => new
            {
                group.Key.IdOpcionReporte,
                group.Key.ReporteNombre,
                Sesiones = group.Count(),
                UsuariosUnicos = group
                    .Select(row => row.IdUsuario)
                    .Distinct()
                    .Count(),
                SegundosVisibles = group.Sum(row => (long)row.SegundosVisibles)
            })
            .OrderByDescending(row => row.SegundosVisibles)
            .ThenByDescending(row => row.Sesiones)
            .ThenBy(row => row.ReporteNombre)
            .Take(MaximoRanking)
            .ToArrayAsync(cancellationToken);

        var usuariosMayorUso = await consulta
            .GroupBy(row => new
            {
                row.IdUsuario,
                row.UsuarioLogin,
                row.UsuarioNombre
            })
            .Select(group => new
            {
                group.Key.IdUsuario,
                group.Key.UsuarioLogin,
                group.Key.UsuarioNombre,
                Sesiones = group.Count(),
                ReportesUnicos = group
                    .Select(row => row.IdOpcionReporte)
                    .Distinct()
                    .Count(),
                SegundosVisibles = group.Sum(row => (long)row.SegundosVisibles)
            })
            .OrderByDescending(row => row.SegundosVisibles)
            .ThenByDescending(row => row.Sesiones)
            .ThenBy(row => row.UsuarioNombre)
            .Take(MaximoRanking)
            .ToArrayAsync(cancellationToken);

        var (granularidad, tendencia) = await ObtenerTendenciaAsync(
            consulta,
            filtro.DesdeUtc,
            filtro.HastaUtc,
            cancellationToken);

        var totalDetalle = await consulta.LongCountAsync(cancellationToken);
        var desplazamiento = (filtro.Pagina - 1) * filtro.TamanoPagina;
        var consultaOrdenada = AplicarOrden(consulta, filtro.Orden);

        var items = await consultaOrdenada
            .Skip(desplazamiento)
            .Take(filtro.TamanoPagina)
            .Select(row => new SesionPowerBiDetalleDatos(
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
                row.MotivoCierre))
            .ToArrayAsync(cancellationToken);

        return new PanelSesionesPowerBiDatos(
            filtro.DesdeUtc,
            filtro.HastaUtc,
            granularidad,
            resumen,
            usoReportes.Select(row => new UsoReportePowerBiDatos(
                    row.IdOpcionReporte,
                    row.ReporteNombre,
                    row.Sesiones,
                    row.UsuariosUnicos,
                    row.SegundosVisibles))
                .ToArray(),
            usuariosMayorUso.Select(row => new UsoUsuarioPowerBiDatos(
                    row.IdUsuario,
                    row.UsuarioLogin,
                    row.UsuarioNombre,
                    row.Sesiones,
                    row.ReportesUnicos,
                    row.SegundosVisibles))
                .ToArray(),
            tendencia,
            catalogos,
            new PaginaSesionesPowerBiDatos(
                filtro.Pagina,
                filtro.TamanoPagina,
                totalDetalle,
                items));
    }

    public async Task<DetalleSesionPowerBiDatos?> ObtenerDetalleAsync(
        Guid idSesion,
        CancellationToken cancellationToken)
    {
        if (idSesion == Guid.Empty)
        {
            return null;
        }

        var sesion = await context.SesionesPowerBiAnalitica
            .AsNoTracking()
            .Where(row => row.IdSesion == idSesion)
            .Select(row => new SesionPowerBiDetalleDatos(
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
                row.MotivoCierre))
            .SingleOrDefaultAsync(cancellationToken);

        if (sesion is null)
        {
            return null;
        }

        var eventos = await context.EventosSesionPowerBiAnalitica
            .AsNoTracking()
            .Where(row => row.IdSesion == idSesion)
            .OrderBy(row => row.FechaEventoUtc)
            .ThenBy(row => row.IdEvento)
            .Select(row => new EventoSesionPowerBiDetalleDatos(
                row.IdEvento,
                row.TipoEvento,
                row.FechaEventoUtc,
                row.SegundosVisibles,
                row.Origen,
                row.Detalle))
            .ToArrayAsync(cancellationToken);

        return new DetalleSesionPowerBiDatos(sesion, eventos);
    }

    private static IQueryable<SesionPowerBiAnalitica> AplicarFiltros(
        IQueryable<SesionPowerBiAnalitica> consulta,
        SesionesPowerBiConsultaFiltro filtro)
    {
        if (filtro.IdOpcionReporte.HasValue)
        {
            consulta = consulta.Where(
                row => row.IdOpcionReporte == filtro.IdOpcionReporte.Value);
        }

        if (filtro.IdUsuario.HasValue)
        {
            consulta = consulta.Where(
                row => row.IdUsuario == filtro.IdUsuario.Value);
        }

        if (filtro.IdCliente.HasValue)
        {
            consulta = consulta.Where(
                row => row.IdCliente == filtro.IdCliente.Value);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
        {
            consulta = consulta.Where(row => row.Estado == filtro.Estado);
        }

        if (!string.IsNullOrWhiteSpace(filtro.Busqueda))
        {
            var busqueda = filtro.Busqueda;
            consulta = consulta.Where(row =>
                row.UsuarioLogin.Contains(busqueda) ||
                row.UsuarioNombre.Contains(busqueda) ||
                row.ReporteNombre.Contains(busqueda) ||
                (row.ClienteNombre != null && row.ClienteNombre.Contains(busqueda)));
        }

        return consulta;
    }

    private static IOrderedQueryable<SesionPowerBiAnalitica> AplicarOrden(
        IQueryable<SesionPowerBiAnalitica> consulta,
        string orden) =>
        orden switch
        {
            "tiempo_desc" => consulta
                .OrderByDescending(row => row.SegundosVisibles)
                .ThenByDescending(row => row.FechaInicioUtc),
            "usuario_asc" => consulta
                .OrderBy(row => row.UsuarioNombre)
                .ThenByDescending(row => row.FechaInicioUtc),
            "reporte_asc" => consulta
                .OrderBy(row => row.ReporteNombre)
                .ThenByDescending(row => row.FechaInicioUtc),
            _ => consulta
                .OrderByDescending(row => row.FechaInicioUtc)
                .ThenByDescending(row => row.FechaCreacionUtc)
        };

    private static async Task<ResumenSesionesPowerBiDatos> ObtenerResumenAsync(
        IQueryable<SesionPowerBiAnalitica> consulta,
        CancellationToken cancellationToken)
    {
        var total = await consulta.LongCountAsync(cancellationToken);
        var activas = await consulta.LongCountAsync(
            row =>
                row.Estado == EstadoSesionPowerBi.Activa &&
                row.EstaVisible,
            cancellationToken);
        var usuarios = await consulta
            .Select(row => row.IdUsuario)
            .Distinct()
            .LongCountAsync(cancellationToken);
        var segundos = await consulta
            .Select(row => (long)row.SegundosVisibles)
            .SumAsync(cancellationToken);
        var promedio = total == 0
            ? 0
            : checked((int)Math.Round(
                (double)segundos / total,
                MidpointRounding.AwayFromZero));

        return new ResumenSesionesPowerBiDatos(
            activas,
            total,
            usuarios,
            segundos,
            promedio);
    }

    private static async Task<CatalogosSesionesPowerBiDatos> ObtenerCatalogosAsync(
        IQueryable<SesionPowerBiAnalitica> consultaPeriodo,
        int? idOpcionReporte,
        CancellationToken cancellationToken)
    {
        // EF Core/SQL Server no puede traducir de forma fiable un GroupBy que
        // proyecta directamente al record de dominio y luego ordena por una
        // propiedad del record. Para los catálogos solo necesitamos valores
        // únicos, por lo que Distinct mantiene toda la operación en SQL y el
        // mapeo al contrato de dominio se realiza después de materializar.
        var reportesRows = await consultaPeriodo
            .Select(row => new
            {
                row.IdOpcionReporte,
                row.ReporteNombre
            })
            .Distinct()
            .OrderBy(row => row.ReporteNombre)
            .ThenBy(row => row.IdOpcionReporte)
            .ToArrayAsync(cancellationToken);

        var usuariosRows = await consultaPeriodo
            .Select(row => new
            {
                row.IdUsuario,
                row.UsuarioNombre
            })
            .Distinct()
            .OrderBy(row => row.UsuarioNombre)
            .ThenBy(row => row.IdUsuario)
            .ToArrayAsync(cancellationToken);

        var consultaClientes = consultaPeriodo;
        if (idOpcionReporte.HasValue)
        {
            consultaClientes = consultaClientes.Where(
                row => row.IdOpcionReporte == idOpcionReporte.Value);
        }

        var clientesRows = await consultaClientes
            .Where(row =>
                row.IdCliente.HasValue &&
                row.ClienteNombre != null)
            .Select(row => new
            {
                IdCliente = row.IdCliente!.Value,
                ClienteNombre = row.ClienteNombre!
            })
            .Distinct()
            .OrderBy(row => row.ClienteNombre)
            .ThenBy(row => row.IdCliente)
            .ToArrayAsync(cancellationToken);

        var reportes = reportesRows
            .Select(row => new OpcionFiltroSesionPowerBiDatos(
                row.IdOpcionReporte,
                row.ReporteNombre))
            .ToArray();

        var usuarios = usuariosRows
            .Select(row => new OpcionFiltroSesionPowerBiDatos(
                row.IdUsuario,
                row.UsuarioNombre))
            .ToArray();

        var clientes = clientesRows
            .Select(row => new OpcionFiltroSesionPowerBiDatos(
                row.IdCliente,
                row.ClienteNombre))
            .ToArray();

        return new CatalogosSesionesPowerBiDatos(
            reportes,
            usuarios,
            clientes);
    }

    private static async Task<(string Granularidad, IReadOnlyList<TendenciaSesionesPowerBiDatos> Items)>
        ObtenerTendenciaAsync(
            IQueryable<SesionPowerBiAnalitica> consulta,
            DateTime desdeUtc,
            DateTime hastaUtc,
            CancellationToken cancellationToken)
    {
        if (hastaUtc - desdeUtc <= TimeSpan.FromDays(2))
        {
            var horas = await consulta
                .GroupBy(row => new
                {
                    row.FechaInicioUtc.Year,
                    row.FechaInicioUtc.Month,
                    row.FechaInicioUtc.Day,
                    row.FechaInicioUtc.Hour
                })
                .Select(group => new
                {
                    group.Key.Year,
                    group.Key.Month,
                    group.Key.Day,
                    group.Key.Hour,
                    Sesiones = group.Count(),
                    UsuariosUnicos = group
                        .Select(row => row.IdUsuario)
                        .Distinct()
                        .Count(),
                    SegundosVisibles = group.Sum(row => (long)row.SegundosVisibles)
                })
                .OrderBy(row => row.Year)
                .ThenBy(row => row.Month)
                .ThenBy(row => row.Day)
                .ThenBy(row => row.Hour)
                .ToArrayAsync(cancellationToken);

            return (
                "HORA",
                horas.Select(row => new TendenciaSesionesPowerBiDatos(
                        new DateTime(
                            row.Year,
                            row.Month,
                            row.Day,
                            row.Hour,
                            0,
                            0,
                            DateTimeKind.Utc),
                        row.Sesiones,
                        row.UsuariosUnicos,
                        row.SegundosVisibles))
                    .ToArray());
        }

        var dias = await consulta
            .GroupBy(row => new
            {
                row.FechaInicioUtc.Year,
                row.FechaInicioUtc.Month,
                row.FechaInicioUtc.Day
            })
            .Select(group => new
            {
                group.Key.Year,
                group.Key.Month,
                group.Key.Day,
                Sesiones = group.Count(),
                UsuariosUnicos = group
                    .Select(row => row.IdUsuario)
                    .Distinct()
                    .Count(),
                SegundosVisibles = group.Sum(row => (long)row.SegundosVisibles)
            })
            .OrderBy(row => row.Year)
            .ThenBy(row => row.Month)
            .ThenBy(row => row.Day)
            .ToArrayAsync(cancellationToken);

        return (
            "DIA",
            dias.Select(row => new TendenciaSesionesPowerBiDatos(
                    new DateTime(
                        row.Year,
                        row.Month,
                        row.Day,
                        0,
                        0,
                        0,
                        DateTimeKind.Utc),
                    row.Sesiones,
                    row.UsuariosUnicos,
                    row.SegundosVisibles))
                .ToArray());
    }
}
