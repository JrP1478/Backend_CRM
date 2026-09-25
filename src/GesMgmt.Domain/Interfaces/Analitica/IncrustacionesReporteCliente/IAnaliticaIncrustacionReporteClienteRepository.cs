using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaIncrustacionReporteClienteRepository
{
    Task<AnaliticaIncrustacionReporteClienteMapeo?> ObtenerAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AnaliticaIncrustacionReporteClienteAdministracionMapeo>> ObtenerActivasPorOpcionAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task GuardarAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        string urlIncrustacion,
        int actualizadoPor,
        CancellationToken cancellationToken);

    Task DesactivarAsync(
        int idOpcion,
        int idClienteCrm,
        string clienteReporte,
        int actualizadoPor,
        CancellationToken cancellationToken);
}
