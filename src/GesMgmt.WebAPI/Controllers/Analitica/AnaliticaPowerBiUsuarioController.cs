using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using Microsoft.AspNetCore.Mvc;

namespace GesMgmt.WebAPI.Controllers.Analitica;

[Route("v1/Analitica/Acceso/Usuario")]
public sealed class AnaliticaPowerBiUsuarioController(
    IContextoUsuarioAnalitica userContext,
    IAccesoUsuarioPowerBiAnaliticaService accessService,
    IOpcionAnaliticaService optionService,
    IContextoVisorPowerBiAnaliticaService contextService) : AnaliticaControllerBase
{
    private const int MaxOptionIds = 100;

    [HttpGet("AccesoPowerBi")]
    [ProducesResponseType(typeof(AnaliticaPowerBiAccesoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerAccesoAsync(
        [FromQuery] string? idOpcions,
        CancellationToken cancellationToken)
    {
        if (!IntentarParsearIdsOpciones(idOpcions, out var normalizedOptionIds))
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Opciones Analítica inválidas",
                $"idOpcions debe contener entre 1 y {MaxOptionIds} enteros positivos separados por coma.");
        }

        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return identityError;
        }

        var access = await accessService.ResolverAsync(
            idUsuario,
            normalizedOptionIds,
            cancellationToken);

        return Ok(new AnaliticaPowerBiAccesoResponse(
            access.Select(option => new AnaliticaPowerBiAccesoOpcionResponse(
                    option.IdOpcion,
                    option.Permitido,
                    option.RequiereSeleccionCliente))
                .ToArray()));
    }

    [HttpGet("Opciones/{idOpcion:int}/ContextoVisorPowerBi")]
    [ProducesResponseType(typeof(AnaliticaContextoVisorPowerBiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObtenerContextoVisorAsync(
        int idOpcion,
        [FromQuery] int? idCliente,
        [FromQuery] string? reportClient,
        CancellationToken cancellationToken)
    {
        if (idOpcion <= 0)
        {
            return ProblemaAnalitica(
                StatusCodes.Status400BadRequest,
                "Opción Analítica inválida",
                "idOpcion debe ser un entero positivo.");
        }

        var identityError = RequerirUsuario(userContext, out var idUsuario);
        if (identityError is not null)
        {
            return identityError;
        }

        if (!await optionService.EstaActivoAsync(idOpcion, cancellationToken))
        {
            return Ok(new AnaliticaContextoVisorPowerBiResponse(
                idOpcion,
                Permitido: false,
                RequiereSeleccionCliente: false,
                EstadoSeleccionCliente: AnaliticaPowerBiClienteSeleccionEstado.NoRequerida,
                ClienteSeleccionado: null,
                UrlIncrustacion: null));
        }

        var selection = ConstruirSeleccion(idCliente, reportClient);
        int? idGrupo = userContext.IntentarObtenerIdGrupo(out var grupoActual)
            ? grupoActual
            : null;
        var context = await contextService.ResolverAsync(
            idUsuario,
            idGrupo,
            idOpcion,
            selection,
            cancellationToken);

        return Ok(new AnaliticaContextoVisorPowerBiResponse(
            idOpcion,
            context.Permitido,
            context.RequiereSeleccionCliente,
            context.EstadoSeleccionCliente,
            context.ClienteSeleccionado,
            context.UrlIncrustacion));
    }

    private static bool IntentarParsearIdsOpciones(
        string? value,
        out int[] idOpcions)
    {
        idOpcions = Array.Empty<int>();

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var parts = value.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        if (parts.Length == 0 || parts.Length > MaxOptionIds)
        {
            return false;
        }

        var parsed = new HashSet<int>();

        foreach (var part in parts)
        {
            if (!int.TryParse(part, out var idOpcion) || idOpcion <= 0)
            {
                return false;
            }

            parsed.Add(idOpcion);
        }

        if (parsed.Count == 0 || parsed.Count > MaxOptionIds)
        {
            return false;
        }

        idOpcions = parsed.OrderBy(idOpcion => idOpcion).ToArray();
        return true;
    }

    private static AnaliticaVisorPowerBiSeleccion? ConstruirSeleccion(
        int? idCliente,
        string? reportClient)
    {
        var name = reportClient?.Trim() ?? string.Empty;

        return idCliente is > 0 && name.Length > 0
            ? new AnaliticaVisorPowerBiSeleccion(idCliente.Value, name)
            : null;
    }
}
