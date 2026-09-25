using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/Acceso/Opciones/{idOpcion:int}/Clientes")]
public sealed class AnaliticaClientesOpcionController(
    IContextoUsuarioAnalitica userContext,
    IAutorizacionAnaliticaService authorizationService,
    IAdministracionClientesOpcionAnaliticaService service) : AnaliticaControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(AnaliticaClientesOpcionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        if (idOpcion <= 0)
        {
            return BadRequest();
        }

        var response = await service.ObtenerAsync(idOpcion, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ActualizarAsync(
        int idOpcion,
        [FromBody] ActualizarAnaliticaClientesOpcionRequest? request,
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
            return BadRequest();
        }

        if (request is null)
        {
            return BadRequest();
        }

        var result = await service.ActualizarAsync(
            idOpcion,
            request.IdsClientes,
            administrator.IdUsuario,
            cancellationToken);

        return ConvertirResultadoComando(result);
    }
}
