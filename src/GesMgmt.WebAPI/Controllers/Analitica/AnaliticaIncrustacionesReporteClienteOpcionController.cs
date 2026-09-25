using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/Acceso/Opciones/{idOpcion:int}/IncrustacionesReporteCliente")]
public sealed class AnaliticaIncrustacionesReporteClienteOpcionController(
    IContextoUsuarioAnalitica userContext,
    IAutorizacionAnaliticaService authorizationService,
    IOpcionAnaliticaService optionService,
    IAdministracionIncrustacionesReporteClienteOpcionAnaliticaService service)
    : AnaliticaControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(AnaliticaIncrustacionesReporteClienteOpcionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var administrator = await RequerirAdministradorAsync(
            userContext,
            authorizationService,
            CrmOptionPermission.Consult,
            cancellationToken);

        if (administrator.Error is not null)
        {
            return administrator.Error;
        }

        var validation = await ValidarOpcionAsync(idOpcion, cancellationToken);
        if (validation is not null)
        {
            return validation;
        }

        return Ok(await service.ObtenerAsync(idOpcion, cancellationToken));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ActualizarAsync(
        int idOpcion,
        [FromBody] ActualizarAnaliticaIncrustacionesReporteClienteOpcionRequest? request,
        CancellationToken cancellationToken)
    {
        var administrator = await RequerirAdministradorAsync(
            userContext,
            authorizationService,
            CrmOptionPermission.Edit,
            cancellationToken);

        if (administrator.Error is not null)
        {
            return administrator.Error;
        }

        var validation = await ValidarOpcionAsync(idOpcion, cancellationToken);
        if (validation is not null)
        {
            return validation;
        }

        if (request is null)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Solicitud inválida",
                "El cuerpo de la solicitud es obligatorio.");
        }

        var result = await service.ActualizarAsync(
            idOpcion,
            request.Publicaciones,
            administrator.IdUsuario,
            cancellationToken);

        return ConvertirResultadoComando(result);
    }

    private async Task<IActionResult?> ValidarOpcionAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (idOpcion <= 0)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Opción Analítica inválida",
                "idOpcion debe ser un entero positivo.");
        }

        return await optionService.ExisteAsync(idOpcion, cancellationToken)
            ? null
            : NotFound();
    }
}
