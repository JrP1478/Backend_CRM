using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Domain.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[ApiController]
public abstract class AnaliticaControllerBase : ControllerBase
{
    public const string NombrePoliticaConcurrenciaCartera =
        "centro-control-cartera-concurrencia";

    public const string NombrePoliticaTimeoutCartera =
        "centro-control-cartera-tiempo-espera";

    protected IActionResult? RequerirUsuario(
        IContextoUsuarioAnalitica userContext,
        out int idUsuario)
    {
        if (userContext.IntentarObtenerIdUsuario(out idUsuario))
        {
            return null;
        }

        return ProblemaAnalitica(
            StatusCodes.Status401Unauthorized,
            "Identidad no disponible",
            "No se pudo identificar al usuario CRM.");
    }

    protected async Task<AccesoAdministradorAnalitica> RequerirAdministradorAsync(
        IContextoUsuarioAnalitica userContext,
        IAutorizacionAnaliticaService authorizationService,
        CrmOptionPermission permiso,
        CancellationToken cancellationToken)
    {
        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return new AccesoAdministradorAnalitica(0, identityError);
        }

        int? idGrupo = userContext.IntentarObtenerIdGrupo(out var currentGroupId)
            ? currentGroupId
            : null;

        var authorization = await authorizationService.PuedeAccederAdministracionAsync(
            idUsuario,
            idGrupo,
            permiso,
            cancellationToken);

        if (!authorization.Permitido)
        {
            return new AccesoAdministradorAnalitica(
                idUsuario,
                ProblemaAnalitica(
                    StatusCodes.Status403Forbidden,
                    "Acceso administrativo denegado",
                    authorization.Reason ??
                    "El usuario no tiene permisos sobre Mantener módulo."));
        }

        return new AccesoAdministradorAnalitica(idUsuario, null);
    }

    protected IActionResult ConvertirResultadoComando(
        AnaliticaAdministracionComandoResult result) =>
        result.Estado switch
        {
            AnaliticaAdministracionComandoEstado.Exito => NoContent(),
            AnaliticaAdministracionComandoEstado.NoEncontrado => NotFound(),
            AnaliticaAdministracionComandoEstado.SolicitudInvalida => ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                result.Titulo ?? "Solicitud inválida",
                result.Detalle ?? "La solicitud no pudo ser procesada."),
            _ => throw new ArgumentOutOfRangeException(
                nameof(result),
                result.Estado,
                "Estado de administración no soportado.")
        };


    protected IActionResult ToPortfolioResult<T>(
        CarteraOperacionResult<T> result)
    {
        if (result.ErroresValidacion is not null)
        {
            return ProblemaValidacionAnalitica(result.ErroresValidacion);
        }

        if (result.EsExitoso)
        {
            return Ok(result.Valor);
        }

        return ProblemaAnalitica(
            result.CodigoEstadoError ?? StatusCodes.Status500InternalServerError,
            result.TituloError ?? "Error de Centro de Control de Cartera",
            result.DetalleError ?? "No se pudo completar la operación solicitada.");
    }

    protected IActionResult ProblemaValidacionAnalitica(
        IReadOnlyDictionary<string, string[]> errors)
    {
        var problem = new ValidationProblemDetails(
            errors.ToDictionary(
                pair => pair.Key,
                pair => pair.Value,
                StringComparer.Ordinal))
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Uno o más errores de validación ocurrieron.",
            Detail = "Revise los parámetros enviados.",
            Instance = HttpContext.Request.Path.Value
        };
        var result = new BadRequestObjectResult(problem);
        result.ContentTypes.Add("application/problem+json");
        return result;
    }

    protected IActionResult ProblemaValidacionAnalitica(ModelStateDictionary modelState)
    {
        var errors = modelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors
                    .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                        ? "El valor proporcionado no es válido."
                        : error.ErrorMessage)
                    .ToArray(),
                StringComparer.Ordinal);

        return ProblemaValidacionAnalitica(errors);
    }

    protected ObjectResult ProblemaAnalitica(
        int codigoEstado,
        string titulo,
        string detalle)
    {
        var problem = new ProblemDetails
        {
            Status = codigoEstado,
            Title = titulo,
            Detail = detalle,
            Instance = HttpContext.Request.Path.Value
        };

        var result = new ObjectResult(problem)
        {
            StatusCode = codigoEstado
        };
        result.ContentTypes.Add("application/problem+json");
        return result;
    }

    protected sealed record AccesoAdministradorAnalitica(
        int IdUsuario,
        IActionResult? Error);
}
