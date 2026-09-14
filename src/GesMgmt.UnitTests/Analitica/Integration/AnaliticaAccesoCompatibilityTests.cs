using System.Data.Common;
using System.Net;
using System.Text.Json;
using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GesMgmt.UnitTests.Analitica.Integration;

public sealed class AnaliticaAccesoCompatibilityTests
{
    [Fact]
    public async Task UserEndpoint_WithoutHostIdentity_ReturnsUnauthorizedProblemDetails()
    {
        await using var factory = new AnaliticaWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/v1/Analitica/Acceso/Usuario/Opciones");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(401, body.RootElement.GetProperty("status").GetInt32());
        Assert.Equal("Identidad no disponible", body.RootElement.GetProperty("title").GetString());
    }

    [Fact]
    public async Task SisgesHeader_AuthenticatesConsolidatedHost()
    {
        await using var factory = CreateHeaderAuthenticatedFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Sisges-User-Id", "16068");

        var response = await client.GetAsync("/v1/Analitica/Acceso/Usuario/Opciones");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UnknownAnalíticaRoute_ReturnsNotFound()
    {
        await using var factory = new AnaliticaWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/v1/Analitica/Acceso/RutaQueNoExiste");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DatabaseFailure_ReturnsInternalServerErrorWithoutLeakingException()
    {
        const string sensitiveMessage = "sensitive database failure";
        await using var factory = CreateThrowingFactory(new TestDbException(sensitiveMessage));
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/v1/Analitica/Acceso/Usuario/Opciones");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.False(body.Contains(sensitiveMessage, StringComparison.Ordinal));
    }

    [Fact]
    public async Task UnexpectedFailure_ReturnsInternalServerErrorWithoutLeakingException()
    {
        const string sensitiveMessage = "sensitive unexpected failure";
        await using var factory = CreateThrowingFactory(new InvalidOperationException(sensitiveMessage));
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/v1/Analitica/Acceso/Usuario/Opciones");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.False(body.Contains(sensitiveMessage, StringComparison.Ordinal));
    }

    private static AnaliticaWebApplicationFactory CreateHeaderAuthenticatedFactory() =>
        new(
            configureTestServices: services =>
            {
                services.RemoveAll<IConsultaOpcionesUsuarioAnaliticaService>();
                services.AddSingleton<IConsultaOpcionesUsuarioAnaliticaService>(new ServicioOpcionesUsuarioVacio());
            });

    private static AnaliticaWebApplicationFactory CreateThrowingFactory(Exception exception) =>
        new(
            configureTestServices: services =>
            {
                services.RemoveAll<IContextoUsuarioAnalitica>();
                services.RemoveAll<IConsultaOpcionesUsuarioAnaliticaService>();
                services.AddSingleton<IContextoUsuarioAnalitica>(new ContextoUsuarioAutenticado(16068));
                services.AddSingleton<IConsultaOpcionesUsuarioAnaliticaService>(new ServicioOpcionesUsuarioConError(exception));
            });

    private sealed class ContextoUsuarioAutenticado(int idUsuario) : IContextoUsuarioAnalitica
    {
        public bool IntentarObtenerIdUsuario(out int currentUserId)
        {
            currentUserId = idUsuario;
            return true;
        }

        public bool IntentarObtenerIdGrupo(out int idGrupo)
        {
            idGrupo = 0;
            return false;
        }
    }

    private sealed class ServicioOpcionesUsuarioVacio : IConsultaOpcionesUsuarioAnaliticaService
    {
        public Task<IReadOnlyList<AnaliticaOpcionUsuarioResponse>> ObtenerAsync(
            int idUsuario,
            CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<AnaliticaOpcionUsuarioResponse>>(Array.Empty<AnaliticaOpcionUsuarioResponse>());
    }

    private sealed class ServicioOpcionesUsuarioConError(Exception exception) : IConsultaOpcionesUsuarioAnaliticaService
    {
        public Task<IReadOnlyList<AnaliticaOpcionUsuarioResponse>> ObtenerAsync(
            int idUsuario,
            CancellationToken cancellationToken) =>
            Task.FromException<IReadOnlyList<AnaliticaOpcionUsuarioResponse>>(exception);
    }

    private sealed class TestDbException(string message) : DbException(message);
}
