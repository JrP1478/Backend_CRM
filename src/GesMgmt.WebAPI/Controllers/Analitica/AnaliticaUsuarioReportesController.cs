using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/Acceso/Usuario/Opciones/{idOpcion:int}")]
public sealed class AnaliticaUsuarioReportesController(
    IContextoUsuarioAnalitica userContext,
    IOpcionAnaliticaService optionService,
    IAccesoReporteClienteAnaliticaService accessService,
    IConsultaIncrustacionReporteClienteAnaliticaService embedService) : AnaliticaControllerBase
{
    [HttpGet("ClientesReporte")]
    [ProducesResponseType(typeof(AnaliticaClientesReporteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerClientesReporteAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var validation = ValidarOpcionYUsuario(idOpcion, out var idUsuario);
        if (validation is not null)
        {
            return validation;
        }

        if (!await optionService.EstaActivoAsync(idOpcion, cancellationToken))
        {
            return NotFound();
        }

        int? idGrupo = userContext.IntentarObtenerIdGrupo(out var grupoActual)
            ? grupoActual
            : null;
        var access = await accessService.ResolverAsync(
            idUsuario,
            idGrupo,
            idOpcion,
            cancellationToken);

        if (!access.TieneAccesoOpcion)
        {
            return ProblemaAnalitica(
                StatusCodes.Status403Forbidden,
                "Acceso al reporte denegado",
                "El usuario no pertenece al grupo habilitado para este reporte.");
        }

        return Ok(new AnaliticaClientesReporteResponse(
            idOpcion,
            access.Clientes));
    }

    [HttpGet("IncrustacionReporteCliente")]
    [ProducesResponseType(typeof(AnaliticaIncrustacionReporteClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerIncrustacionClienteReporteAsync(
        int idOpcion,
        [FromQuery] int idCliente,
        [FromQuery] string? reportClient,
        CancellationToken cancellationToken)
    {
        var requestedName = reportClient?.Trim() ?? string.Empty;

        if (idOpcion <= 0 || idCliente <= 0 || requestedName.Length == 0)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Selección de cartera inválida",
                "idOpcion, idCliente y reportClient son obligatorios y deben ser válidos.");
        }

        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return identityError;
        }

        if (!await optionService.EstaActivoAsync(idOpcion, cancellationToken))
        {
            return NotFound();
        }

        int? idGrupo = userContext.IntentarObtenerIdGrupo(out var grupoActual)
            ? grupoActual
            : null;
        var access = await accessService.ResolverAsync(
            idUsuario,
            idGrupo,
            idOpcion,
            cancellationToken);

        if (!access.TieneAccesoOpcion)
        {
            return ProblemaAnalitica(
                StatusCodes.Status403Forbidden,
                "Acceso al reporte denegado",
                "El usuario no pertenece al grupo habilitado para este reporte.");
        }

        var authorizedClient = access.Clientes.FirstOrDefault(client =>
            client.IdCliente == idCliente &&
            string.Equals(
                client.Nombre.Trim(),
                requestedName,
                StringComparison.OrdinalIgnoreCase));

        if (authorizedClient is null)
        {
            return ProblemaAnalitica(
                StatusCodes.Status403Forbidden,
                "Cartera no autorizada",
                "La cartera solicitada no está habilitada para el usuario.");
        }

        var embed = await embedService.ResolverAsync(
            idOpcion,
            authorizedClient.IdCliente,
            authorizedClient.Nombre,
            cancellationToken);

        if (embed.Estado == AnaliticaIncrustacionReporteClienteConsultaEstado.NoEncontrado)
        {
            return ProblemaAnalitica(
                StatusCodes.Status404NotFound,
                "Publicación no configurada",
                "La cartera todavía no tiene una URL de publicación web asignada para este reporte.");
        }

        if (embed.Estado == AnaliticaIncrustacionReporteClienteConsultaEstado.ConfiguracionInvalida)
        {
            return ProblemaAnalitica(
                StatusCodes.Status500InternalServerError,
                "Publicación Power BI inválida",
                "La URL configurada para la cartera no es una publicación válida de Power BI.");
        }

        return Ok(new AnaliticaIncrustacionReporteClienteResponse(
            idOpcion,
            authorizedClient.IdCliente,
            authorizedClient.Nombre,
            embed.UrlIncrustacion!));
    }

    private IActionResult? ValidarOpcionYUsuario(
        int idOpcion,
        out int idUsuario)
    {
        idUsuario = 0;

        if (idOpcion <= 0)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Opción Analítica inválida",
                "idOpcion debe ser un entero positivo.");
        }

        return RequerirUsuario(userContext, out idUsuario);
    }
}
