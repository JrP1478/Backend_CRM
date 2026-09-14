using Microsoft.Extensions.DependencyInjection;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class RendimientoAsesorCarteraCacheRepository(
    RendimientoAsesorCarteraRepository requestRepository,
    IServiceScopeFactory scopeFactory,
    ICacheRendimientoCartera cache,
    RendimientoCentroControlCarteraOptions performance)
    : IRendimientoAsesorCarteraRepository
{
    private readonly TimeSpan _cacheDuration =
        TimeSpan.FromSeconds(performance.SegundosCacheDetalle);

    public Task<IReadOnlyList<RendimientoAsesorCarteraDbFila>?> ObtenerRendimientoAsesorAsync(
        int idClienteCrm,
        RendimientoAsesorCarteraRequest request,
        CancellationToken cancellationToken)
    {
        if (_cacheDuration <= TimeSpan.Zero)
        {
            return requestRepository.ObtenerRendimientoAsesorAsync(
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

    private async Task<IReadOnlyList<RendimientoAsesorCarteraDbFila>?> ExecuteCacheMissAsync(
        int idClienteCrm,
        RendimientoAsesorCarteraRequest request,
        CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var repository = scope.ServiceProvider
            .GetRequiredService<RendimientoAsesorCarteraRepository>();

        return await repository.ObtenerRendimientoAsesorAsync(
            idClienteCrm,
            request,
            cancellationToken);
    }

    internal static string ConstruirClaveCache(
        int idClienteCrm,
        RendimientoAsesorCarteraRequest request) =>
        string.Create(
            System.Globalization.CultureInfo.InvariantCulture,
            $"pcc:advisor:v2:{idClienteCrm}:{request.UnidadNegocio ?? "*"}:{request.Campana ?? "*"}:{request.IdSubCartera?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "*"}:{request.IdSupervisor?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "*"}:{request.FechaDesde?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) ?? "*"}:{request.FechaHasta?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture) ?? "*"}");
}
