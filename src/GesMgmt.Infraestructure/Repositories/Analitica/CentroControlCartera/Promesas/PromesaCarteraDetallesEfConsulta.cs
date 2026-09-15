using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal static class PromesaCarteraDetallesEfConsulta
{
    public static IQueryable<PromesaOperativaAnalitica> AplicarAlcance(
        AnaliticaDbContext context,
        IQueryable<PromesaOperativaAnalitica> query,
        long? idSubCartera,
        string? unidadNegocio)
    {
        if (idSubCartera.HasValue)
        {
            query = query.Where(row => row.ClaveCartera == idSubCartera.Value);
        }

        if (unidadNegocio is null)
        {
            return query;
        }

        var portfolioKeys = context.CarterasAnalitica
            .AsNoTracking()
            .Where(portfolio => portfolio.UnidadNegocioOrigen == unidadNegocio)
            .Select(portfolio => portfolio.ClaveCartera);

        return query.Where(row => portfolioKeys.Contains(row.ClaveCartera));
    }

    public static IQueryable<PromesaOperativaSupervisorAnalitica> AplicarAlcance(
        AnaliticaDbContext context,
        IQueryable<PromesaOperativaSupervisorAnalitica> query,
        long? idSubCartera,
        string? unidadNegocio)
    {
        if (idSubCartera.HasValue)
        {
            query = query.Where(row => row.ClaveCartera == idSubCartera.Value);
        }

        if (unidadNegocio is null)
        {
            return query;
        }

        var portfolioKeys = context.CarterasAnalitica
            .AsNoTracking()
            .Where(portfolio => portfolio.UnidadNegocioOrigen == unidadNegocio)
            .Select(portfolio => portfolio.ClaveCartera);

        return query.Where(row => portfolioKeys.Contains(row.ClaveCartera));
    }

    public static decimal RedondearMonto(decimal value) =>
        decimal.Round(value, 4, MidpointRounding.AwayFromZero);

    public static string? NormalizarNombre(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static int ObtenerOffset(int pagina, int tamanoPagina) =>
        checked((pagina - 1) * tamanoPagina);
}
