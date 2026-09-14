using Microsoft.Extensions.DependencyInjection;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class RendimientoSupervisorCarteraCacheRepository(
    RendimientoSupervisorCarteraRepository requestRepository,
    IServiceScopeFactory scopeFactory,
    ICacheRendimientoCartera cache,
    RendimientoCentroControlCarteraOptions performance)
    : IRendimientoSupervisorCarteraRepository
{
    private readonly TimeSpan _cacheDuration =
        TimeSpan.FromSeconds(performance.SegundosCacheDetalle);

    public Task<IReadOnlyList<RendimientoSupervisorCarteraDbFila>?> ObtenerRendimientoSupervisorAsync(
        int idClienteCrm,
        RendimientoSupervisorCarteraRequest request,
        CancellationToken cancellationToken)
    {
        if (_cacheDuration <= TimeSpan.Zero)
        {
            return requestRepository.ObtenerRendimientoSupervisorAsync(
                idClienteCrm,
                request,
                cancellationToken);
        }

        return cache.GetOrCreateAsync(
            ConstruirClaveCache(idClienteCrm, request),
            _cacheDuration,
            token => ExecuteCacheMissAsync(
                idClienteCrm,
                request,
                token),
            cancellationToken);
    }

    private async Task<IReadOnlyList<RendimientoSupervisorCarteraDbFila>?> ExecuteCacheMissAsync(
        int idClienteCrm,
        RendimientoSupervisorCarteraRequest request,
        CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var repository = scope.ServiceProvider
            .GetRequiredService<RendimientoSupervisorCarteraRepository>();

        return await repository.ObtenerRendimientoSupervisorAsync(
            idClienteCrm,
            request,
            cancellationToken);
    }

    internal static string ConstruirClaveCache(
        int idClienteCrm,
        RendimientoSupervisorCarteraRequest request) =>
        string.Create(
            System.Globalization.CultureInfo.InvariantCulture,
            $"pcc:supervisor:v2:{idClienteCrm}:{request.UnidadNegocio ?? "*"}:{request.Campana ?? "*"}:{request.IdSubCartera?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "*"}:{request.IdSupervisor?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "*"}:{request.FechaDesde?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) ?? "*"}:{request.FechaHasta?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) ?? "*"}");
}
