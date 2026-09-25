using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/CentroControlCartera/Promesas/Vencidas")]
[EnableRateLimiting(AnaliticaControllerBase.NombrePoliticaConcurrenciaCartera)]
[RequestTimeout(AnaliticaControllerBase.NombrePoliticaTimeoutCartera)]
public sealed class PromesasVencidasCarteraController(IPromesasVencidasCarteraService service) : AnaliticaControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PromesasVencidasCarteraResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerAsync(
        [FromQuery] string? campana,
        [FromQuery] string? idSubCartera,
        [FromQuery] string? pagina,
        [FromQuery] string? tamanoPagina,
        [FromQuery] string? antiguedad,
        [FromQuery] string? ordenarPor,
        [FromQuery] string? direccionOrden,
        [FromQuery] string? unidadNegocio,
        [FromQuery] int? idClienteCrm,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ProblemaValidacionAnalitica(ModelState);
        }

        var result = await service.ObtenerAsync(
            campana,
            idSubCartera,
            pagina,
            tamanoPagina,
            antiguedad,
            ordenarPor,
            direccionOrden,
            unidadNegocio,
            idClienteCrm,
            cancellationToken);

        return ToPortfolioResult(result);
    }
}
