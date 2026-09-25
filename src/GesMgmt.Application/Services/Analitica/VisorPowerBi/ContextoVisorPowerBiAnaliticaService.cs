using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class ContextoVisorPowerBiAnaliticaService(
    IAccesoOpcionAnaliticaService accesoOpcionService,
    IConfiguracionReporteClienteAnaliticaService configuracionService,
    ICrmOpcionPermisoRepository permisoRepository)
    : IContextoVisorPowerBiAnaliticaService
{
    public async Task<AnaliticaContextoVisorPowerBi> ResolverAsync(
        int idUsuario,
        int? idGrupo,
        int idOpcion,
        AnaliticaVisorPowerBiSeleccion? seleccion,
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

        var tienePermisoCrm = await permisoRepository.TienePermisoAsync(
            idUsuario,
            idGrupo,
            idOpcion,
            CrmOptionPermission.Consult,
            cancellationToken);

        if (!tienePermisoCrm)
        {
            return Denegado();
        }

        var requiereSeleccionCliente =
            await configuracionService.RequiereSeleccionClienteAsync(
                idOpcion,
                cancellationToken);

        /*
         * Un reporte con URL directa se autoriza por la opción CRM.
         * Los alcances específicos de Analítica solamente son necesarios
         * cuando el BI se segmenta por cliente/cartera.
         */
        if (!requiereSeleccionCliente)
        {
            return new AnaliticaContextoVisorPowerBi(
                Permitido: true,
                RequiereSeleccionCliente: false,
                EstadoSeleccionCliente: AnaliticaPowerBiClienteSeleccionEstado.NoRequerida,
                ClienteSeleccionado: null,
                UrlIncrustacion: null);
        }

        var omiteValidacionAlcanceGrupoOpcion =
            AnaliticaReporteAccesoPolicy.OmiteValidacionAlcanceGrupoOpcion(idOpcion);
        var accesoOpcion = await accesoOpcionService.ResolverAsync(
            idUsuario,
            idOpcion,
            cancellationToken);

        if (!omiteValidacionAlcanceGrupoOpcion && !accesoOpcion.Permitido)
        {
            return Denegado();
        }

        if (seleccion is null)
        {
            return new AnaliticaContextoVisorPowerBi(
                Permitido: true,
                RequiereSeleccionCliente: true,
                EstadoSeleccionCliente: AnaliticaPowerBiClienteSeleccionEstado.Faltante,
                ClienteSeleccionado: null,
                UrlIncrustacion: null);
        }

        var idsGruposUsuarioActivos = accesoOpcion.IdsGruposUsuarioActivos.ToHashSet();
        var configuraciones = await configuracionService.ResolverAsync(
            idOpcion,
            cancellationToken);

        var configuracionAutorizada = configuraciones.FirstOrDefault(configuracion =>
            AnaliticaReporteClienteAutorizacion.Coincide(
                configuracion,
                seleccion.IdCliente,
                seleccion.Nombre) &&
            AnaliticaReporteClienteAutorizacion.EstaAutorizado(
                configuracion,
                idsGruposUsuarioActivos));

        if (configuracionAutorizada is null)
        {
            return new AnaliticaContextoVisorPowerBi(
                Permitido: true,
                RequiereSeleccionCliente: true,
                EstadoSeleccionCliente: AnaliticaPowerBiClienteSeleccionEstado.Invalido,
                ClienteSeleccionado: null,
                UrlIncrustacion: null);
        }

        if (!UrlPublicacionWebPowerBi.IntentarNormalizar(
            configuracionAutorizada.UrlIncrustacion,
            out var urlIncrustacion))
        {
            return new AnaliticaContextoVisorPowerBi(
                Permitido: true,
                RequiereSeleccionCliente: true,
                EstadoSeleccionCliente: AnaliticaPowerBiClienteSeleccionEstado.Invalido,
                ClienteSeleccionado: null,
                UrlIncrustacion: null);
        }

        return new AnaliticaContextoVisorPowerBi(
            Permitido: true,
            RequiereSeleccionCliente: true,
            EstadoSeleccionCliente: AnaliticaPowerBiClienteSeleccionEstado.Valida,
            ClienteSeleccionado: new AnaliticaOpcionReporteCliente(
                configuracionAutorizada.IdCliente,
                configuracionAutorizada.Nombre),
            UrlIncrustacion: urlIncrustacion);
    }

    private static AnaliticaContextoVisorPowerBi Denegado() =>
        new(
            Permitido: false,
            RequiereSeleccionCliente: false,
            EstadoSeleccionCliente: AnaliticaPowerBiClienteSeleccionEstado.NoRequerida,
            ClienteSeleccionado: null,
            UrlIncrustacion: null);
}
