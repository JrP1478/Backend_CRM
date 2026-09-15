using Microsoft.Extensions.Configuration;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed class RendimientoCentroControlCarteraOptions
{
    public const string NombreSeccion = "PortfolioControlCenter:Performance";

    public const int MaximoSolicitudesConcurrentesPredeterminado = 80;
    public const int LimiteColaPredeterminado = 240;
    public const int SegundosTiempoEsperaSolicitudPredeterminado = 60;
    public const int SegundosCacheDetallePredeterminado = 30;
    public const int MaximoEntradasCacheDetallePredeterminado = 512;

    [ConfigurationKeyName("MaxConcurrentRequests")]
    public int MaximoSolicitudesConcurrentes { get; init; } = MaximoSolicitudesConcurrentesPredeterminado;
    [ConfigurationKeyName("QueueLimit")]
    public int LimiteCola { get; init; } = LimiteColaPredeterminado;
    [ConfigurationKeyName("RequestTimeoutSeconds")]
    public int SegundosTiempoEsperaSolicitud { get; init; } = SegundosTiempoEsperaSolicitudPredeterminado;
    [ConfigurationKeyName("DetailCacheSeconds")]
    public int SegundosCacheDetalle { get; init; } = SegundosCacheDetallePredeterminado;
    [ConfigurationKeyName("DetailCacheMaxEntries")]
    public int MaximoEntradasCacheDetalle { get; init; } = MaximoEntradasCacheDetallePredeterminado;

    public void Validar()
    {
        if (MaximoSolicitudesConcurrentes is < 1 or > 512)
        {
            throw new InvalidOperationException(
                $"{NombreSeccion}:MaxConcurrentRequests debe estar entre 1 y 512.");
        }

        if (LimiteCola is < 0 or > 4096)
        {
            throw new InvalidOperationException(
                $"{NombreSeccion}:QueueLimit debe estar entre 0 y 4096.");
        }

        if (SegundosTiempoEsperaSolicitud is < 1 or > 300)
        {
            throw new InvalidOperationException(
                $"{NombreSeccion}:RequestTimeoutSeconds debe estar entre 1 y 300.");
        }

        if (SegundosCacheDetalle is < 0 or > 300)
        {
            throw new InvalidOperationException(
                $"{NombreSeccion}:DetailCacheSeconds debe estar entre 0 y 300.");
        }

        if (MaximoEntradasCacheDetalle is < 1 or > 4096)
        {
            throw new InvalidOperationException(
                $"{NombreSeccion}:DetailCacheMaxEntries debe estar entre 1 y 4096.");
        }
    }
}
