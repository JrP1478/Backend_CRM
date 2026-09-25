using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAdministracionIncrustacionesReporteClienteOpcionAnaliticaService
{
    Task<AnaliticaIncrustacionesReporteClienteOpcionResponse> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyList<ActualizarAnaliticaIncrustacionReporteClienteOpcion>? publicaciones,
        int adminUserId,
        CancellationToken cancellationToken);
}
