using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
namespace GesMgmt.Application.Services.Analitica.CentroControlCartera;

internal static class PanoramaCarteraResponseMapper
{
    public static PanoramaCarteraResponse Map(
        ResumenCarteraContexto context,
        RangoResumenCartera range,
        PanoramaCarteraDbFilas rows)
    {
        var targetContext = new AvanceMetaCarteraContexto(
            context.ClaveCliente,
            context.ClaveCampana,
            context.CodigoCampana,
            context.NombreCampana,
            context.FechaInicio,
            context.FechaFin,
            context.FechaUltimoDato);

        var promisesContext = new PromesasCarteraContexto(
            context.ClaveCliente,
            context.ClaveCampana,
            context.CodigoCampana,
            context.NombreCampana);

        var evolutionContext = new EvolucionCarteraContexto(
            context.ClaveCliente,
            context.ClaveCampana,
            context.CodigoCampana,
            context.NombreCampana,
            context.FechaInicio,
            context.FechaFin,
            context.FechaUltimoDato);

        return new PanoramaCarteraResponse(
            ResumenCarteraResponseMapper.Map(
                context,
                range,
                rows.Resumen),
            AvanceMetaCarteraResponseMapper.Map(
                targetContext,
                range.FechaHasta,
                rows.TargetProgress),
            PromesasCarteraResponseMapper.Map(
                promisesContext,
                rows.Promises),
            EvolucionCarteraResponseMapper.Map(
                evolutionContext,
                new RangoEvolucionCartera(
                    range.FechaDesde,
                    range.FechaHasta),
                rows.Evolucion));
    }
}
