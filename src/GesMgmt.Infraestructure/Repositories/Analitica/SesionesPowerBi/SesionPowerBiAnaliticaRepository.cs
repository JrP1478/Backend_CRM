using GesMgmt.Domain.Constants.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.SesionesPowerBi;

internal sealed class SesionPowerBiAnaliticaRepository(AnaliticaDbContext context)
    : ISesionPowerBiAnaliticaRepository
{
    private const string OrigenFrontend = "FRONTEND";

    public async Task CrearAsync(
        SesionPowerBiAnalitica sesion,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        context.SesionesPowerBiAnalitica.Add(sesion);
        context.EventosSesionPowerBiAnalitica.Add(new EventoSesionPowerBiAnalitica
        {
            IdSesion = sesion.IdSesion,
            TipoEvento = TipoEventoSesionPowerBi.Open,
            FechaEventoUtc = sesion.FechaInicioUtc,
            SegundosVisibles = 0,
            Origen = OrigenFrontend,
            Detalle = "Apertura del visor Power BI."
        });

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<ResultadoMutacionSesionPowerBi> ActualizarActividadAsync(
        Guid idSesion,
        int idUsuario,
        int segundosVisibles,
        bool visible,
        DateTime ahoraUtc,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var actual = await context.SesionesPowerBiAnalitica
            .AsNoTracking()
            .Where(row => row.IdSesion == idSesion && row.IdUsuario == idUsuario)
            .Select(row => new
            {
                row.Estado,
                row.EstaVisible,
                row.SegundosVisibles
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (actual is null)
        {
            return new ResultadoMutacionSesionPowerBi(false, false);
        }

        if (EstadoSesionPowerBi.EsFinal(actual.Estado))
        {
            return new ResultadoMutacionSesionPowerBi(true, true);
        }

        var estado = visible
            ? EstadoSesionPowerBi.Activa
            : EstadoSesionPowerBi.Pausada;

        var updated = await context.SesionesPowerBiAnalitica
            .Where(row =>
                row.IdSesion == idSesion &&
                row.IdUsuario == idUsuario &&
                row.Estado != EstadoSesionPowerBi.Cerrada &&
                row.Estado != EstadoSesionPowerBi.Expirada)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(
                        row => row.SegundosVisibles,
                        row => row.SegundosVisibles < segundosVisibles
                            ? segundosVisibles
                            : row.SegundosVisibles)
                    .SetProperty(row => row.EstaVisible, visible)
                    .SetProperty(row => row.Estado, estado)
                    .SetProperty(row => row.FechaUltimoHeartbeatUtc, ahoraUtc)
                    .SetProperty(row => row.FechaActualizacionUtc, ahoraUtc),
                cancellationToken);

        if (updated == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new ResultadoMutacionSesionPowerBi(true, true);
        }

        if (actual.EstaVisible != visible)
        {
            context.EventosSesionPowerBiAnalitica.Add(new EventoSesionPowerBiAnalitica
            {
                IdSesion = idSesion,
                TipoEvento = visible
                    ? TipoEventoSesionPowerBi.Visible
                    : TipoEventoSesionPowerBi.Hidden,
                FechaEventoUtc = ahoraUtc,
                SegundosVisibles = Math.Max(actual.SegundosVisibles, segundosVisibles),
                Origen = OrigenFrontend,
                Detalle = visible
                    ? "El visor volvió a estar visible."
                    : "El visor dejó de estar visible."
            });

            await context.SaveChangesAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        return new ResultadoMutacionSesionPowerBi(true, false);
    }

    public async Task<ResultadoMutacionSesionPowerBi> CerrarAsync(
        Guid idSesion,
        int idUsuario,
        int segundosVisibles,
        string motivoCierre,
        DateTime ahoraUtc,
        CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var actual = await context.SesionesPowerBiAnalitica
            .AsNoTracking()
            .Where(row => row.IdSesion == idSesion && row.IdUsuario == idUsuario)
            .Select(row => new
            {
                row.Estado,
                row.SegundosVisibles
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (actual is null)
        {
            return new ResultadoMutacionSesionPowerBi(false, false);
        }

        if (EstadoSesionPowerBi.EsFinal(actual.Estado))
        {
            return new ResultadoMutacionSesionPowerBi(true, true);
        }

        var updated = await context.SesionesPowerBiAnalitica
            .Where(row =>
                row.IdSesion == idSesion &&
                row.IdUsuario == idUsuario &&
                row.Estado != EstadoSesionPowerBi.Cerrada &&
                row.Estado != EstadoSesionPowerBi.Expirada)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(
                        row => row.SegundosVisibles,
                        row => row.SegundosVisibles < segundosVisibles
                            ? segundosVisibles
                            : row.SegundosVisibles)
                    .SetProperty(row => row.EstaVisible, false)
                    .SetProperty(row => row.Estado, EstadoSesionPowerBi.Cerrada)
                    .SetProperty(row => row.FechaFinUtc, ahoraUtc)
                    .SetProperty(row => row.MotivoCierre, motivoCierre)
                    .SetProperty(row => row.FechaUltimoHeartbeatUtc, ahoraUtc)
                    .SetProperty(row => row.FechaActualizacionUtc, ahoraUtc),
                cancellationToken);

        if (updated == 0)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new ResultadoMutacionSesionPowerBi(true, true);
        }

        context.EventosSesionPowerBiAnalitica.Add(new EventoSesionPowerBiAnalitica
        {
            IdSesion = idSesion,
            TipoEvento = TipoEventoSesionPowerBi.Close,
            FechaEventoUtc = ahoraUtc,
            SegundosVisibles = Math.Max(actual.SegundosVisibles, segundosVisibles),
            Origen = OrigenFrontend,
            Detalle = $"Cierre del visor. Motivo: {motivoCierre}."
        });

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return new ResultadoMutacionSesionPowerBi(true, false);
    }
}
