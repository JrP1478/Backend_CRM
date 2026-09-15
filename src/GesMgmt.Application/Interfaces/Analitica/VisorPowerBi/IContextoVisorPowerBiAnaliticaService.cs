using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IContextoVisorPowerBiAnaliticaService
{
    Task<AnaliticaContextoVisorPowerBi> ResolverAsync(
        int idUsuario,
        int? idGrupo,
        int idOpcion,
        AnaliticaVisorPowerBiSeleccion? selection,
        CancellationToken cancellationToken);
}
