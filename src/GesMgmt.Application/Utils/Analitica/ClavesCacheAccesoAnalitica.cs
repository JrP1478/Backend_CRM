using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Utils.Analitica;

public static class ClavesCacheAccesoAnalitica
{
    public const string ActiveOptions =
        "analytics-access:options:active";

    public static string CatalogoClienteReporte(int idOpcion) =>
        $"analytics-access:report-client-catalog:{idOpcion}";

    public static string PublicacionesClienteReporte(int idOpcion) =>
        $"analytics-access:report-client-publicaciones:{idOpcion}";
}
