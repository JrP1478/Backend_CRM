using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AdministracionIncrustacionesReporteClienteOpcionAnaliticaService(
    IConfiguracionReporteClienteAnaliticaService configurationService,
    IAnaliticaPublicacionReporteClienteWriter publicationWriter,
    ISeguridadPowerBiAnaliticaPolicy powerBiSecurityPolicy)
    : IAdministracionIncrustacionesReporteClienteOpcionAnaliticaService
{
    public async Task<AnaliticaIncrustacionesReporteClienteOpcionResponse> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var configurations = await configurationService.ResolverAsync(
            idOpcion,
            cancellationToken);
        var clientes = configurations
            .Select(AnaliticaConfiguracionReporteClienteContratoMapper.Map)
            .ToArray();

        return new AnaliticaIncrustacionesReporteClienteOpcionResponse(
            idOpcion,
            clientes);
    }

    public async Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyList<ActualizarAnaliticaIncrustacionReporteClienteOpcion>? publicaciones,
        int adminUserId,
        CancellationToken cancellationToken)
    {
        var configurations = await configurationService.ResolverAsync(
            idOpcion,
            cancellationToken);
        var publicationValidation = AnaliticaPublicacionReporteClienteActualizarValidator.Validar(
            publicaciones,
            configurations,
            powerBiSecurityPolicy.PermitirPublicarEnWeb);

        if (publicationValidation.Error is not null)
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                publicationValidation.Error.Titulo,
                publicationValidation.Error.Detalle);
        }

        await publicationWriter.PatchAsync(
            idOpcion,
            publicationValidation.Updates,
            adminUserId,
            cancellationToken);

        return AnaliticaAdministracionComandoResult.Exito();
    }
}
