using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IConfiguracionPowerBiAnaliticaService
{
    Task<AnaliticaConfiguracionPowerBiResponse> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken);

    Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        ActualizarAnaliticaConfiguracionPowerBiRequest request,
        int idUsuario,
        CancellationToken cancellationToken);
}
