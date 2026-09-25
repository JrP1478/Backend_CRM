using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaCatalogoReporteClienteRepository
{
    bool Soporta(int idOpcion);

    Task<IReadOnlyList<AnaliticaCatalogoReporteClienteItem>> ObtenerActualAsync(
        int idOpcion,
        CancellationToken cancellationToken);
}
