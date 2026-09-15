using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Interfaces.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Constants.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;

namespace GesMgmt.Application.Services.Analitica.SesionesPowerBi;

public sealed class SesionPowerBiAnaliticaService(
    IOpcionAnaliticaService opcionService,
    IContextoVisorPowerBiAnaliticaService contextoVisorService,
    ICatalogoSesionPowerBiRepository catalogoRepository,
    ISesionPowerBiAnaliticaRepository sesionRepository)
    : ISesionPowerBiAnaliticaService
{
    private const int MaxSegundosVisiblesSesion = 86_400;

    private static readonly HashSet<string> MotivosCierrePermitidos =
        new(StringComparer.Ordinal)
        {
            "NAVEGACION",
            "PAGEHIDE",
            "LOGOUT",
            "ERROR",
            "CAMBIO_REPORTE"
        };

    public async Task<SesionPowerBiOperacionResultado> AbrirAsync(
        int idUsuario,
        int? idGrupo,
        AbrirSesionPowerBiRequest request,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 || request.IdOpcion <= 0)
        {
            return Invalida("IdOpcion debe ser un entero positivo.");
        }

        var reportClient = request.ReportClient?.Trim();
        var hasClientId = request.IdCliente is > 0;
        var hasClientName = !string.IsNullOrWhiteSpace(reportClient);

        if (hasClientId != hasClientName)
        {
            return Invalida(
                "IdCliente y ReportClient deben enviarse juntos cuando el reporte requiere selección de cliente.");
        }

        if (reportClient is { Length: > 250 })
        {
            return Invalida("ReportClient no puede superar 250 caracteres.");
        }

        if (!await opcionService.EstaActivoAsync(request.IdOpcion, cancellationToken))
        {
            return NoEncontrado("El reporte solicitado no está disponible en Gestión Analítica.");
        }

        var seleccion = hasClientId
            ? new AnaliticaVisorPowerBiSeleccion(request.IdCliente!.Value, reportClient!)
            : null;

        var contexto = await contextoVisorService.ResolverAsync(
            idUsuario,
            idGrupo,
            request.IdOpcion,
            seleccion,
            cancellationToken);

        if (!contexto.Permitido)
        {
            return Denegado("El usuario no tiene acceso al reporte solicitado.");
        }

        if (contexto.RequiereSeleccionCliente &&
            contexto.EstadoSeleccionCliente != AnaliticaPowerBiClienteSeleccionEstado.Valida)
        {
            return contexto.EstadoSeleccionCliente == AnaliticaPowerBiClienteSeleccionEstado.Invalido
                ? Denegado("El cliente seleccionado no está autorizado para este reporte.")
                : Invalida("El reporte requiere seleccionar un cliente antes de iniciar la sesión.");
        }

        var snapshot = await catalogoRepository.ObtenerSnapshotAsync(
            idUsuario,
            request.IdOpcion,
            cancellationToken);

        if (snapshot is null)
        {
            return NoEncontrado("No se pudo resolver el usuario o el reporte en SISGES.");
        }

        var ahoraUtc = DateTime.UtcNow;
        var idSesion = Guid.NewGuid();
        var cliente = contexto.RequiereSeleccionCliente
            ? contexto.ClienteSeleccionado
            : null;

        var sesion = new SesionPowerBiAnalitica
        {
            IdSesion = idSesion,
            IdUsuario = snapshot.IdUsuario,
            IdOpcionReporte = snapshot.IdOpcionReporte,
            IdCliente = cliente?.IdCliente,
            UsuarioLogin = snapshot.UsuarioLogin,
            UsuarioNombre = snapshot.UsuarioNombre,
            ReporteNombre = snapshot.ReporteNombre,
            ClienteNombre = cliente?.Nombre,
            FechaInicioUtc = ahoraUtc,
            FechaUltimoHeartbeatUtc = ahoraUtc,
            FechaFinUtc = null,
            SegundosVisibles = 0,
            EstaVisible = true,
            Estado = EstadoSesionPowerBi.Activa,
            MotivoCierre = null,
            FechaCreacionUtc = ahoraUtc,
            FechaActualizacionUtc = ahoraUtc
        };

        await sesionRepository.CrearAsync(sesion, cancellationToken);

        return new SesionPowerBiOperacionResultado(
            SesionPowerBiOperacionEstado.Exito,
            new AbrirSesionPowerBiResponse(idSesion, ahoraUtc));
    }

    public async Task<SesionPowerBiOperacionResultado> ActualizarActividadAsync(
        int idUsuario,
        Guid idSesion,
        ActualizarActividadSesionPowerBiRequest request,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 || idSesion == Guid.Empty)
        {
            return Invalida("La sesión indicada no es válida.");
        }

        if (!SegundosValidos(request.SegundosVisibles))
        {
            return Invalida(
                $"SegundosVisibles debe estar entre 0 y {MaxSegundosVisiblesSesion}.");
        }

        var resultado = await sesionRepository.ActualizarActividadAsync(
            idSesion,
            idUsuario,
            request.SegundosVisibles,
            request.Visible,
            DateTime.UtcNow,
            cancellationToken);

        if (!resultado.Encontrada)
        {
            return NoEncontrado("La sesión BI no existe o no pertenece al usuario actual.");
        }

        if (resultado.Finalizada)
        {
            return new SesionPowerBiOperacionResultado(
                SesionPowerBiOperacionEstado.Finalizada,
                Titulo: "Sesión finalizada",
                Detalle: "La sesión BI ya fue cerrada o expirada y no acepta nuevos heartbeats.");
        }

        return new SesionPowerBiOperacionResultado(SesionPowerBiOperacionEstado.Exito);
    }

    public async Task<SesionPowerBiOperacionResultado> CerrarAsync(
        int idUsuario,
        Guid idSesion,
        CerrarSesionPowerBiRequest request,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 || idSesion == Guid.Empty)
        {
            return Invalida("La sesión indicada no es válida.");
        }

        if (!SegundosValidos(request.SegundosVisibles))
        {
            return Invalida(
                $"SegundosVisibles debe estar entre 0 y {MaxSegundosVisiblesSesion}.");
        }

        var motivo = NormalizarMotivoCierre(request.MotivoCierre);
        if (motivo is null)
        {
            return Invalida("MotivoCierre no contiene un valor permitido.");
        }

        var resultado = await sesionRepository.CerrarAsync(
            idSesion,
            idUsuario,
            request.SegundosVisibles,
            motivo,
            DateTime.UtcNow,
            cancellationToken);

        if (!resultado.Encontrada)
        {
            return NoEncontrado("La sesión BI no existe o no pertenece al usuario actual.");
        }

        // El cierre es idempotente: repetirlo sobre una sesión ya finalizada es éxito.
        return new SesionPowerBiOperacionResultado(SesionPowerBiOperacionEstado.Exito);
    }

    private static bool SegundosValidos(int segundos) =>
        segundos is >= 0 and <= MaxSegundosVisiblesSesion;

    private static string? NormalizarMotivoCierre(string? motivo)
    {
        var normalized = string.IsNullOrWhiteSpace(motivo)
            ? "NAVEGACION"
            : motivo.Trim().ToUpperInvariant();

        return MotivosCierrePermitidos.Contains(normalized)
            ? normalized
            : null;
    }

    private static SesionPowerBiOperacionResultado Invalida(string detalle) =>
        new(
            SesionPowerBiOperacionEstado.SolicitudInvalida,
            Titulo: "Solicitud de sesión BI inválida",
            Detalle: detalle);

    private static SesionPowerBiOperacionResultado NoEncontrado(string detalle) =>
        new(
            SesionPowerBiOperacionEstado.NoEncontrado,
            Titulo: "Sesión BI no disponible",
            Detalle: detalle);

    private static SesionPowerBiOperacionResultado Denegado(string detalle) =>
        new(
            SesionPowerBiOperacionEstado.Denegado,
            Titulo: "Acceso al reporte denegado",
            Detalle: detalle);
}
