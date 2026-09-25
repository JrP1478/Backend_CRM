using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Domain.Interfaces.Analitica;

public interface IAnaliticaPublicacionReporteClienteWriter
{
    Task PatchAsync(
        int idOpcion,
        IReadOnlyCollection<AnaliticaPublicacionReporteClienteActualizar> publicaciones,
        int actualizadoPor,
        CancellationToken cancellationToken);
}
