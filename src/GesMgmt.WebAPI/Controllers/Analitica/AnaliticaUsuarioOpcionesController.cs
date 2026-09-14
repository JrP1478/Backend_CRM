using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/Acceso/Usuario/Opciones")]
public sealed class AnaliticaUsuarioOpcionesController(
    IContextoUsuarioAnalitica userContext,
    IConsultaOpcionesUsuarioAnaliticaService userOptionService,
    IAccesoAnaliticaService accessService,
    IOpcionAnaliticaService optionService,
    IAccesoOpcionAnaliticaService optionAccessService,
    IConfiguracionReporteClienteAnaliticaService reportClientConfigurationService)
    : AnaliticaControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AnaliticaOpcionUsuarioResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerAsync(CancellationToken cancellationToken)
    {
        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return identityError;
        }

        return Ok(await userOptionService.ObtenerAsync(idUsuario, cancellationToken));
    }

    [HttpGet("{idOpcion:int}/Clientes")]
    [ProducesResponseType(typeof(AnaliticaUsuarioClientesPermitidosResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerClientesPermitidosAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var validation = ValidarOpcionYUsuario(idOpcion, out var idUsuario);
        if (validation is not null)
        {
            return validation;
        }

        var clientes = await accessService.ObtenerClientesPermitidosAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        return Ok(AnaliticaUsuarioClientesPermitidosResponse.DesdeClientes(
            idOpcion,
            clientes));
    }

    [HttpGet("{idOpcion:int}/AlcancesCliente")]
    [ProducesResponseType(typeof(AnaliticaUsuarioClientesPermitidosResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerAlcancesClienteAsync(
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
            return Ok(AnaliticaUsuarioClientesPermitidosResponse.DesdeIds(
                idOpcion,
                Array.Empty<int>()));
        }

        var idsClientes = await accessService.ObtenerIdsClientesAlcanceClienteAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        return Ok(AnaliticaUsuarioClientesPermitidosResponse.DesdeIds(
            idOpcion,
            idsClientes));
    }

    [HttpGet("{idOpcion:int}/AlcancesGrupo")]
    [ProducesResponseType(typeof(AnaliticaUsuarioPermitidoGruposResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerAlcancesGrupoAsync(
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
            return Ok(new AnaliticaUsuarioPermitidoGruposResponse(
                idOpcion,
                Permitido: false,
                ModoAlcance: "NONE",
                IdsGrupos: Array.Empty<int>(),
                RequiereSeleccionCliente: false));
        }

        var requiresClientSelection =
            await reportClientConfigurationService.RequiereSeleccionClienteAsync(
                idOpcion,
                cancellationToken);
        var access = await optionAccessService.ResolverAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        return Ok(new AnaliticaUsuarioPermitidoGruposResponse(
            idOpcion,
            access.Permitido,
            access.ModoAlcance,
            access.IdsGruposCoincidentes,
            requiresClientSelection));
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
