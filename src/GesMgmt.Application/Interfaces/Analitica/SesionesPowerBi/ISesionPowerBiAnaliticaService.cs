using GesMgmt.Application.DTOs.Analitica.SesionesPowerBi;

namespace GesMgmt.Application.Interfaces.Analitica.SesionesPowerBi;

public interface ISesionPowerBiAnaliticaService
{
    Task<SesionPowerBiOperacionResultado> AbrirAsync(
        int idUsuario,
        int? idGrupo,
        AbrirSesionPowerBiRequest request,
        CancellationToken cancellationToken);

    Task<SesionPowerBiOperacionResultado> ActualizarActividadAsync(
        int idUsuario,
        Guid idSesion,
        ActualizarActividadSesionPowerBiRequest request,
        CancellationToken cancellationToken);

    Task<SesionPowerBiOperacionResultado> CerrarAsync(
        int idUsuario,
        Guid idSesion,
        CerrarSesionPowerBiRequest request,
        CancellationToken cancellationToken);
}
