using Microsoft.Extensions.Configuration;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed class SeguridadPowerBiAnaliticaOptions
{
    public const string NombreSeccion = "AnalyticsPowerBiSecurity";

    // Publish to web is public by design. Keep this switch enabled only while
    // the application intentionally relies on public Power BI publicaciones.
    [ConfigurationKeyName("AllowPublishToWeb")]
    public bool PermitirPublicarEnWeb { get; init; } = true;
}
