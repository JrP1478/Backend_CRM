using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaConfiguracionPowerBiWriter
{
    Task ActualizarAsync(
        int idOpcion,
        string codigoOpcion,
        string nombreOpcion,
        bool esActivo,
        IReadOnlyCollection<int> previousGroupIds,
        IReadOnlyCollection<int> idsGrupos,
        IReadOnlyCollection<AnaliticaPublicacionReporteClienteActualizar> publicaciones,
        int actualizadoPor,
        CancellationToken cancellationToken);
}
