using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Utils.Analitica;

public static class PoliticaCacheAccesoAnalitica
{
    // Solo configuración compartida y de baja volatilidad. Los permisos de usuario,
    // scopes de cliente/grupo y datos SISGES se mantienen fuera de cache.
    public static readonly TimeSpan ConfigurationDuration =
        TimeSpan.FromSeconds(30);

    public static readonly TimeSpan CatalogDuration =
        TimeSpan.FromMinutes(1);
}
