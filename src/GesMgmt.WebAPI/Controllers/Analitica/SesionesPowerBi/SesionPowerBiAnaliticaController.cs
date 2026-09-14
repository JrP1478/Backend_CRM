using GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Interfaces.Analitica.SesionesPowerBi;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica.SesionesPowerBi;

[Route("v1/Analitica/PowerBi/Sesiones")]
public sealed class SesionPowerBiAnaliticaController(
    IContextoUsuarioAnalitica userContext,
    ISesionPowerBiAnaliticaService service,
    IConsultaSesionesPowerBiService consultaService) : AnaliticaControllerBase
{
    [HttpGet("Panel")]
    [ProducesResponseType(typeof(PanelSesionesPowerBiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPanelAsync(
        [FromQuery] DateTimeOffset? desdeUtc,
        [FromQuery] DateTimeOffset? hastaUtc,
        [FromQuery] int? idOpcionReporte,
        [FromQuery] int? idUsuario,
        [FromQuery] int? idCliente,
        [FromQuery] string? estado,
        [FromQuery] string? busqueda,
        [FromQuery] string? orden,
        [FromQuery] int? pagina,
        [FromQuery] int? tamanoPagina,
        CancellationToken cancellationToken)
    {
        var identityError = RequerirUsuario(userContext, out var idUsuarioActual);
        if (identityError is not null)
        {
            return identityError;
        }

        int? idGrupo = userContext.IntentarObtenerIdGrupo(out var grupoActual)
            ? grupoActual
            : null;

        var result = await consultaService.ObtenerPanelAsync(
            idUsuarioActual,
            idGrupo,
            new ConsultarPanelSesionesPowerBiRequest(
                desdeUtc,
                hastaUtc,
                idOpcionReporte,
                idUsuario,
                idCliente,
                estado,
                busqueda,
                orden,
                pagina,
                tamanoPagina),
            cancellationToken);

        return ConvertirConsulta(result);
    }

    [HttpGet("{idSesion:guid}")]
    [ProducesResponseType(typeof(DetalleSesionPowerBiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDetalleAsync(
        Guid idSesion,
        CancellationToken cancellationToken)
    {
        var identityError = RequerirUsuario(userContext, out var idUsuarioActual);
        if (identityError is not null)
        {
            return identityError;
        }

        int? idGrupo = userContext.IntentarObtenerIdGrupo(out var grupoActual)
            ? grupoActual
            : null;

        var result = await consultaService.ObtenerDetalleAsync(
            idUsuarioActual,
            idGrupo,
            idSesion,
            cancellationToken);

        return ConvertirDetalle(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AbrirSesionPowerBiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AbrirAsync(
        [FromBody] AbrirSesionPowerBiRequest request,
        CancellationToken cancellationToken)
    {
        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return identityError;
        }

        int? idGrupo = userContext.IntentarObtenerIdGrupo(out var grupoActual)
            ? grupoActual
            : null;

        var result = await service.AbrirAsync(
            idUsuario,
            idGrupo,
            request,
            cancellationToken);
        return Convertir(result, created: true);
    }

    [HttpPut("{idSesion:guid}/Actividad")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ActualizarActividadAsync(
        Guid idSesion,
        [FromBody] ActualizarActividadSesionPowerBiRequest request,
        CancellationToken cancellationToken)
    {
        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return identityError;
        }

        var result = await service.ActualizarActividadAsync(
            idUsuario,
            idSesion,
            request,
            cancellationToken);

        return Convertir(result);
    }

    [HttpPost("{idSesion:guid}/Cerrar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CerrarAsync(
        Guid idSesion,
        [FromBody] CerrarSesionPowerBiRequest request,
        CancellationToken cancellationToken)
    {
        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return identityError;
        }

        var result = await service.CerrarAsync(
            idUsuario,
            idSesion,
            request,
            cancellationToken);

        return Convertir(result);
    }


    private IActionResult ConvertirDetalle(
        ConsultaDetalleSesionPowerBiOperacionResultado result) =>
        result.Estado switch
        {
            ConsultaDetalleSesionPowerBiOperacionEstado.Exito => Ok(result.DetalleSesion),
            ConsultaDetalleSesionPowerBiOperacionEstado.SolicitudInvalida => ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                result.Titulo ?? "Consulta inválida",
                result.Detalle ?? "No se pudo consultar el detalle de la sesión BI."),
            ConsultaDetalleSesionPowerBiOperacionEstado.NoEncontrado => NotFound(),
            ConsultaDetalleSesionPowerBiOperacionEstado.Denegado => ProblemaAnalitica(
                StatusCodes.Status403Forbidden,
                result.Titulo ?? "Acceso denegado",
                result.Detalle ?? "El usuario no puede consultar Sesiones BI."),
            _ => throw new InvalidOperationException(
                $"Estado de detalle de sesión BI no soportado: {result.Estado}.")
        };

    private IActionResult ConvertirConsulta(
        ConsultaSesionesPowerBiOperacionResultado result) =>
        result.Estado switch
        {
            ConsultaSesionesPowerBiOperacionEstado.Exito => Ok(result.Panel),
            ConsultaSesionesPowerBiOperacionEstado.SolicitudInvalida => ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                result.Titulo ?? "Consulta inválida",
                result.Detalle ?? "No se pudo consultar la telemetría de Sesiones BI."),
            ConsultaSesionesPowerBiOperacionEstado.Denegado => ProblemaAnalitica(
                StatusCodes.Status403Forbidden,
                result.Titulo ?? "Acceso denegado",
                result.Detalle ?? "El usuario no puede consultar Sesiones BI."),
            _ => throw new InvalidOperationException(
                $"Estado de consulta de sesiones BI no soportado: {result.Estado}.")
        };

    private IActionResult Convertir(
        SesionPowerBiOperacionResultado result,
        bool created = false) =>
        result.Estado switch
        {
            SesionPowerBiOperacionEstado.Exito when created =>
                StatusCode(StatusCodes.Status201Created, result.Apertura),
            SesionPowerBiOperacionEstado.Exito => NoContent(),
            SesionPowerBiOperacionEstado.SolicitudInvalida => ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                result.Titulo ?? "Solicitud inválida",
                result.Detalle ?? "La solicitud de sesión BI no es válida."),
            SesionPowerBiOperacionEstado.NoEncontrado => NotFound(),
            SesionPowerBiOperacionEstado.Denegado => ProblemaAnalitica(
                StatusCodes.Status403Forbidden,
                result.Titulo ?? "Acceso denegado",
                result.Detalle ?? "El usuario no puede registrar esta sesión BI."),
            SesionPowerBiOperacionEstado.Finalizada => ProblemaAnalitica(
                StatusCodes.Status409Conflict,
                result.Titulo ?? "Sesión finalizada",
                result.Detalle ?? "La sesión BI ya no acepta actividad."),
            _ => throw new InvalidOperationException(
                $"Estado de sesión BI no soportado: {result.Estado}.")
        };
}
