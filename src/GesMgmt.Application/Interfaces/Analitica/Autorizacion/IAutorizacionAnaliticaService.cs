using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Constants;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAutorizacionAnaliticaService
{
    Task<AnaliticaAutorizacionResult> PuedeAccederAdministracionAsync(
        int idUsuario,
        int? idGrupo,
        SisgesOptionPermission permiso,
        CancellationToken cancellationToken);
}
