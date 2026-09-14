using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public static class AnaliticaConfiguracionReporteClienteContratoMapper
{
    public static AnaliticaIncrustacionReporteClienteOpcion Map(
        AnaliticaConfiguracionReporteCliente configuration) =>
        new(
            configuration.IdCliente,
            configuration.Nombre,
            configuration.EstaDisponible,
            configuration.GroupResolution,
            configuration.TieneConfiguracionGruposExplicita,
            configuration.IdsGrupos,
            configuration.GruposCandidatos
                .Select(group => new AnaliticaOpcionReporteClienteGrupo(
                    group.IdGrupo,
                    group.Nombre))
                .ToArray(),
            string.IsNullOrWhiteSpace(configuration.UrlIncrustacion)
                ? null
                : configuration.UrlIncrustacion.Trim(),
            configuration.EstaLista);
}
