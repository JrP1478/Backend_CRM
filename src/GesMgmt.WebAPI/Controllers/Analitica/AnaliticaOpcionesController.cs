using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/Acceso/Opciones")]
public sealed class AnaliticaOpcionesController(
    IContextoUsuarioAnalitica userContext,
    IAutorizacionAnaliticaService authorizationService,
    IOpcionAnaliticaService optionService) : AnaliticaControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AnaliticaOpcionConfiguracionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerAsync(CancellationToken cancellationToken)
    {
        var administrator = await RequerirAdministradorAsync(
            userContext,
            authorizationService,
            SisgesOptionPermission.Consult,
            cancellationToken);

        if (administrator.Error is not null)
        {
            return administrator.Error;
        }

        var options = await optionService.ObtenerTodosAsync(cancellationToken);
        return Ok(options);
    }

    [HttpPut("{idOpcion:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ActualizarAsync(
        int idOpcion,
        [FromBody] GuardarAnaliticaOpcionRequest? request,
        CancellationToken cancellationToken)
    {
        var administrator = await RequerirAdministradorAsync(
            userContext,
            authorizationService,
            SisgesOptionPermission.Edit,
            cancellationToken);

        if (administrator.Error is not null)
        {
            return administrator.Error;
        }

        if (idOpcion <= 0)
        {
            return BadRequest();
        }

        if (request is null)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Datos de opción inválidos",
                "El cuerpo de la solicitud es obligatorio.");
        }

        var codigoOpcion = request.CodigoOpcion?.Trim();
        var nombreOpcion = request.NombreOpcion?.Trim();

        if (string.IsNullOrWhiteSpace(codigoOpcion) ||
            string.IsNullOrWhiteSpace(nombreOpcion))
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Datos de opción inválidos",
                "codigoOpcion y nombreOpcion son obligatorios.");
        }

        await optionService.GuardarAsync(
            idOpcion,
            codigoOpcion,
            nombreOpcion,
            request.EsActivo,
            administrator.IdUsuario,
            cancellationToken);

        return NoContent();
    }
}
