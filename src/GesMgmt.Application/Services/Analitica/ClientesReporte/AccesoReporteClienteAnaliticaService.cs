using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AccesoReporteClienteAnaliticaService(
    IAccesoOpcionAnaliticaService optionAccessService,
    IConfiguracionReporteClienteAnaliticaService configurationService)
    : IAccesoReporteClienteAnaliticaService
{
    public async Task<AnaliticaReporteClienteAccesoResult> ResolverAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idUsuario));
        }

        if (idOpcion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idOpcion));
        }

        var optionAccess = await optionAccessService.ResolverAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        if (!optionAccess.Permitido)
        {
            return new AnaliticaReporteClienteAccesoResult(
                false,
                Array.Empty<AnaliticaOpcionReporteCliente>());
        }

        var activeUserGroupSet = optionAccess.IdsGruposUsuarioActivos.ToHashSet();
        var configurations = await configurationService.ResolverAsync(
            idOpcion,
            cancellationToken);

        var clientes = configurations
            .Where(configuration =>
                AnaliticaReporteClienteAutorizacion.EstaAutorizado(
                    configuration,
                    activeUserGroupSet))
            .Select(configuration => new AnaliticaOpcionReporteCliente(
                configuration.IdCliente,
                configuration.Nombre))
            .OrderBy(client => client.Nombre, StringComparer.OrdinalIgnoreCase)
            .ThenBy(client => client.IdCliente)
            .ToArray();

        return new AnaliticaReporteClienteAccesoResult(true, clientes);
    }
}
