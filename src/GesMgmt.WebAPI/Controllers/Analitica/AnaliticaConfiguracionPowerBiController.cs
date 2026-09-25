using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/Acceso/Opciones/{idOpcion:int}/ConfiguracionPowerBi")]
public sealed class AnaliticaConfiguracionPowerBiController(
    IContextoUsuarioAnalitica userContext,
    IAutorizacionAnaliticaService authorizationService,
    IConfiguracionPowerBiAnaliticaService service) : AnaliticaControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(AnaliticaConfiguracionPowerBiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
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

        if (idOpcion <= 0)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Opción Analítica inválida",
                "idOpcion debe ser un entero positivo.");
        }

        return Ok(await service.ObtenerAsync(idOpcion, cancellationToken));
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PatchAsync(
        int idOpcion,
        [FromBody] ActualizarAnaliticaConfiguracionPowerBiRequest? request,
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

        if (idOpcion <= 0)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Opción Analítica inválida",
                "idOpcion debe ser un entero positivo.");
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
            request,
            administrator.IdUsuario,
            cancellationToken);

        return ConvertirResultadoComando(result);
    }
}
