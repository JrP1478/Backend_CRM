using Microsoft.Extensions.Configuration;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed class AdministracionAnaliticaOptions
{
    public const string NombreSeccion = "AnalyticsAdministration";

    [ConfigurationKeyName("AdministratorUserIds")]
    public int[] IdsUsuariosAdministradores { get; init; } = [];
}
