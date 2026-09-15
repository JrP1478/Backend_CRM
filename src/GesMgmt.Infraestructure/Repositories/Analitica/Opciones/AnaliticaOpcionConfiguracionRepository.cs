using System.Data;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaOpcionConfiguracionRepository(
    AnaliticaDbContext context,
    ICacheAccesoAnalitica cache)
    : IAnaliticaOpcionConfiguracionRepository
{
    public Task<bool> ExisteAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (idOpcion <= 0)
        {
            return Task.FromResult(false);
        }

        return context.ConfiguracionesOpcionAnalitica
            .AsNoTracking()
            .AnyAsync(
                option => option.IdOpcion == idOpcion,
                cancellationToken);
    }

    public async Task<bool> EstaActivoAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (idOpcion <= 0)
        {
            return false;
        }

        var options = await ObtenerOpcionesActivasAsync(cancellationToken);
        return options.Any(option => option.IdOpcion == idOpcion);
    }

    public Task<IReadOnlyList<ConfiguracionOpcionAnalitica>> ObtenerTodosAsync(
        CancellationToken cancellationToken) =>
        ObtenerOpcionesActivasAsync(cancellationToken);

    public async Task GuardarAsync(
        int idOpcion,
        string codigoOpcion,
        string nombreOpcion,
        bool esActivo,
        int? idUsuario,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var option = await context.ConfiguracionesOpcionAnalitica
            .SingleOrDefaultAsync(
                current => current.IdOpcion == idOpcion,
                cancellationToken);

        var now = DateTime.UtcNow;

        if (option is null)
        {
            option = new ConfiguracionOpcionAnalitica
            {
                IdOpcion = idOpcion,
                CodigoOpcion = codigoOpcion,
                NombreOpcion = nombreOpcion,
                EsActivo = esActivo,
                CreadoPor = idUsuario,
                FechaCreacion = now
            };

            await context.ConfiguracionesOpcionAnalitica.AddAsync(
                option,
                cancellationToken);
        }
        else
        {
            option.CodigoOpcion = codigoOpcion;
            option.NombreOpcion = nombreOpcion;
            option.EsActivo = esActivo;
            option.ActualizadoPor = idUsuario;
            option.FechaActualizacion = now;
        }

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        cache.Remove(ClavesCacheAccesoAnalitica.ActiveOptions);
    }

    private Task<IReadOnlyList<ConfiguracionOpcionAnalitica>> ObtenerOpcionesActivasAsync(
        CancellationToken cancellationToken) =>
        cache.GetOrCreateAsync<IReadOnlyList<ConfiguracionOpcionAnalitica>>(
            ClavesCacheAccesoAnalitica.ActiveOptions,
            PoliticaCacheAccesoAnalitica.ConfigurationDuration,
            async token => await context.ConfiguracionesOpcionAnalitica
                .AsNoTracking()
                .Where(option => option.EsActivo)
                .OrderBy(option => option.IdOpcion)
                .Select(option => new ConfiguracionOpcionAnalitica
                {
                    IdOpcion = option.IdOpcion,
                    CodigoOpcion = option.CodigoOpcion,
                    NombreOpcion = option.NombreOpcion,
                    EsActivo = true
                })
                .ToArrayAsync(token),
            cancellationToken);
}
