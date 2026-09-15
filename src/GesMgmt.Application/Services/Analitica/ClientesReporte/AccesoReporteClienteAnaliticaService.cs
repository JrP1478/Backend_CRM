using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AccesoReporteClienteAnaliticaService(
    IAccesoOpcionAnaliticaService optionAccessService,
    IConfiguracionReporteClienteAnaliticaService configurationService,
    ISisgesOpcionPermisoRepository permissionRepository)
    : IAccesoReporteClienteAnaliticaService
{
    public async Task<AnaliticaReporteClienteAccesoResult> ResolverAsync(
        int idUsuario,
        int? idGrupo,
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

        var omiteValidacionAlcanceGrupoOpcion =
            AnaliticaReporteAccesoPolicy.OmiteValidacionAlcanceGrupoOpcion(idOpcion);

        if (omiteValidacionAlcanceGrupoOpcion)
        {
            var tienePermisoSisges = await permissionRepository.TienePermisoAsync(
                idUsuario,
                idGrupo,
                idOpcion,
                SisgesOptionPermission.Consult,
                cancellationToken);

            if (!tienePermisoSisges)
            {
                return Denegado();
            }
        }

        var optionAccess = await optionAccessService.ResolverAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        if (!omiteValidacionAlcanceGrupoOpcion && !optionAccess.Permitido)
        {
            return Denegado();
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

    private static AnaliticaReporteClienteAccesoResult Denegado() =>
        new(
            false,
            Array.Empty<AnaliticaOpcionReporteCliente>());
}
