using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;

namespace GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;

public interface ISesionPowerBiAnaliticaRepository
{
    Task CrearAsync(
        SesionPowerBiAnalitica sesion,
        CancellationToken cancellationToken);

    Task<ResultadoMutacionSesionPowerBi> ActualizarActividadAsync(
        Guid idSesion,
        int idUsuario,
        int segundosVisibles,
        bool visible,
        DateTime ahoraUtc,
        CancellationToken cancellationToken);

    Task<ResultadoMutacionSesionPowerBi> CerrarAsync(
        Guid idSesion,
        int idUsuario,
        int segundosVisibles,
        string motivoCierre,
        DateTime ahoraUtc,
        CancellationToken cancellationToken);
}
