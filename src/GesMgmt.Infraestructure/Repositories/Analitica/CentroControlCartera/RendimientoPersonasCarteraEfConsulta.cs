using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal static class RendimientoPersonasCarteraEfConsulta
{
    private static readonly string[] EstadosCumplidos =
    [
        "FULFILLED",
        "PARTIAL",
        "FULFILLED_OUT_OF_RANGE"
    ];

    private static readonly string[] EstadosCumplimiento =
    [
        "FULFILLED",
        "PARTIAL",
        "FULFILLED_OUT_OF_RANGE",
        "BROKEN"
    ];

    public static async Task<IReadOnlyList<RendimientoAsesorCarteraDbFila>?> ObtenerAsesoresAsync(
        AnaliticaDbContext context,
        int idClienteCrm,
        RendimientoAsesorCarteraRequest request,
        CancellationToken cancellationToken)
    {
        var claveCliente = await ResolverClaveClienteAsync(
            context,
            idClienteCrm,
            request.UnidadNegocio,
            cancellationToken);

        if (!claveCliente.HasValue)
        {
            return null;
        }

        var campaignKeys = await ObtenerClavesCampanaAsync(
            context,
            claveCliente.Value,
            request.Campana,
            cancellationToken);

        if (campaignKeys.Length == 0)
        {
            return [];
        }

        var activity = ConstruirConsultaActividad(
            context,
            claveCliente.Value,
            campaignKeys,
            request.IdSubCartera,
            request.UnidadNegocio,
            request.IdSupervisor,
            request.FechaDesde,
            request.FechaHasta);

        var managementRows = await activity
            .GroupBy(row => row.ClaveAsesor)
            .Select(group => new MetricasGestionAsesor(
                group.Key,
                group.Max(row => row.NombreAsesor),
                group.Min(row => row.FechaCalendario),
                group.Max(row => row.FechaCalendario),
                group.Sum(row => (long)(row.EventosGestion ?? 0)),
                group.Sum(row => row.MontoRecuperado ?? 0m),
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        if (managementRows.Count == 0)
        {
            return [];
        }

        var effectiveDateFrom = managementRows.Min(row => row.FechaDesde);
        var effectiveDateTo = managementRows.Max(row => row.FechaHasta);

        var supervisorAssignments = await activity
            .Where(row => row.ClaveSupervisor.HasValue)
            .Select(row => new
            {
                row.ClaveAsesor,
                ClaveSupervisor = row.ClaveSupervisor!.Value,
                row.NombreSupervisor
            })
            .Distinct()
            .ToListAsync(cancellationToken);

        var contactRows = await ConstruirConsultaContacto(
                context,
                claveCliente.Value,
                campaignKeys,
                request.IdSubCartera,
                request.UnidadNegocio,
                request.IdSupervisor,
                effectiveDateFrom,
                effectiveDateTo)
            .Where(row => row.ClaveAsesor.HasValue)
            .GroupBy(row => new
            {
                ClaveAsesor = row.ClaveAsesor!.Value,
                row.ClaveCampana,
                row.ClaveCartera,
                row.IdDeudorOrigen
            })
            .Select(group => new MetricasContactoDeudor(
                group.Key.ClaveAsesor,
                group.Any(row => row.TuvoContactoDirecto),
                group.Any(row => row.TuvoContactoIndirecto),
                group.Any(row => row.TuvoSinContacto),
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var promiseRows = await ConstruirConsultaPromesa(
                context,
                claveCliente.Value,
                campaignKeys,
                request.IdSubCartera,
                request.UnidadNegocio,
                request.IdSupervisor,
                effectiveDateFrom,
                effectiveDateTo)
            .Where(row => row.ClaveAsesor.HasValue && row.EsPromesaValida)
            .GroupBy(row => new
            {
                ClaveAsesor = row.ClaveAsesor!.Value,
                row.ClaveCampana,
                row.ClaveCartera,
                row.IdDeudorOrigen
            })
            .Select(group => new MetricasPromesaDeudor(
                group.Key.ClaveAsesor,
                group.LongCount(),
                0m,
                0m,
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var paymentRows = await ConstruirConsultaPago(
                context,
                claveCliente.Value,
                campaignKeys,
                request.IdSubCartera,
                request.UnidadNegocio,
                request.IdSupervisor,
                effectiveDateFrom,
                effectiveDateTo)
            .Where(row => row.ClaveAsesor.HasValue)
            .GroupBy(row => new
            {
                ClaveAsesor = row.ClaveAsesor!.Value,
                row.ClaveCampana,
                row.ClaveCartera,
                row.IdDeudorOrigen
            })
            .Select(group => new MetricasPagoDeudor(
                group.Key.ClaveAsesor,
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var currentSupervisors = await context.SupervisorActualAsesorAnalitica
            .AsNoTracking()
            .Where(row => row.ClaveCliente == claveCliente.Value)
            .ToListAsync(cancellationToken);

        var contactByAdvisor = contactRows
            .GroupBy(row => row.ActorKey)
            .ToDictionary(
                group => group.Key,
                group => new MetricasContacto(
                    group.LongCount(),
                    group.LongCount(row => row.TuvoContactoDirecto),
                    group.LongCount(row =>
                        row.TuvoContactoDirecto
                        || row.TuvoContactoIndirecto
                        || row.TuvoSinContacto),
                    MaximoNullable(group.Select(row => row.FechaCarga))));

        var promiseByAdvisor = promiseRows
            .GroupBy(row => row.ActorKey)
            .ToDictionary(
                group => group.Key,
                group => new MetricasPromesa(
                    group.Sum(row => row.CantidadPromesas),
                    group.LongCount(),
                    0m,
                    0m,
                    MaximoNullable(group.Select(row => row.FechaCarga))));

        var paymentByAdvisor = paymentRows
            .GroupBy(row => row.ActorKey)
            .ToDictionary(
                group => group.Key,
                group => new MetricasPago(
                    group.LongCount(),
                    MaximoNullable(group.Select(row => row.FechaCarga))));

        var supervisorByAdvisor = supervisorAssignments
            .GroupBy(row => row.ClaveAsesor)
            .ToDictionary(group => group.Key, group =>
            {
                var supervisorKeys = group
                    .Select(row => row.ClaveSupervisor)
                    .Distinct()
                    .ToArray();

                if (supervisorKeys.Length != 1)
                {
                    return new AsignacionSupervisor(null, null);
                }

                var claveSupervisor = supervisorKeys[0];
                var nombreSupervisor = group
                    .Where(row => row.ClaveSupervisor == claveSupervisor)
                    .Select(row => NormalizarNombre(row.NombreSupervisor))
                    .Where(name => name is not null)
                    .OrderByDescending(name => name, StringComparer.Ordinal)
                    .FirstOrDefault();

                return new AsignacionSupervisor(claveSupervisor, nombreSupervisor);
            });

        var currentSupervisorByAdvisor = currentSupervisors
            .GroupBy(row => row.ClaveAsesor)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(row => new AsignacionSupervisor(
                        row.ClaveSupervisor,
                        NormalizarNombre(row.NombreSupervisor)))
                    .First());

        return managementRows
            .Select(management =>
            {
                contactByAdvisor.TryGetValue(management.ClaveAsesor, out var contact);
                promiseByAdvisor.TryGetValue(management.ClaveAsesor, out var promise);
                paymentByAdvisor.TryGetValue(management.ClaveAsesor, out var payment);
                supervisorByAdvisor.TryGetValue(management.ClaveAsesor, out var periodSupervisor);
                currentSupervisorByAdvisor.TryGetValue(management.ClaveAsesor, out var currentSupervisor);

                return new RendimientoAsesorCarteraDbFila
                {
                    IdAsesor = management.ClaveAsesor,
                    NombreAsesor = management.NombreAsesor,
                    IdSupervisorPeriodo = periodSupervisor?.ClaveSupervisor,
                    NombreSupervisorPeriodo = periodSupervisor?.NombreSupervisor,
                    IdSupervisorActual = currentSupervisor?.ClaveSupervisor,
                    NombreSupervisorActual = currentSupervisor?.NombreSupervisor,
                    FechaDesde = effectiveDateFrom,
                    FechaHasta = effectiveDateTo,
                    CantidadGestiones = management.CantidadGestiones,
                    CantidadDeudoresGestionados = contact?.ManagedDebtors ?? 0,
                    TasaContactoDirecto = Dividir(
                        contact?.DirectContactClients ?? 0,
                        contact?.ClassifiableClients ?? 0),
                    TasaCierre = Dividir(
                        promise?.ClientesPromesaValida ?? 0,
                        contact?.DirectContactClients ?? 0),
                    CantidadPromesas = promise?.CantidadPromesas ?? 0,
                    CantidadPagos = payment?.CantidadPagos ?? 0,
                    MontoRecuperadoAtribuible = Redondear(management.MontoRecuperado, 4),
                    FechaActualizacionUtc = MaximoNullable(
                    [
                        management.FechaCarga,
                        contact?.FechaCarga,
                        promise?.FechaCarga,
                        payment?.FechaCarga
                    ])
                };
            })
            .OrderBy(row => row.NombreAsesor, StringComparer.Ordinal)
            .ThenBy(row => row.IdAsesor)
            .ToArray();
    }

    public static async Task<IReadOnlyList<RendimientoSupervisorCarteraDbFila>?> ObtenerSupervisoresAsync(
        AnaliticaDbContext context,
        int idClienteCrm,
        RendimientoSupervisorCarteraRequest request,
        CancellationToken cancellationToken)
    {
        var claveCliente = await ResolverClaveClienteAsync(
            context,
            idClienteCrm,
            request.UnidadNegocio,
            cancellationToken);

        if (!claveCliente.HasValue)
        {
            return null;
        }

        var campaignKeys = await ObtenerClavesCampanaAsync(
            context,
            claveCliente.Value,
            request.Campana,
            cancellationToken);

        if (campaignKeys.Length == 0)
        {
            return [];
        }

        var activity = ConstruirConsultaActividad(
                context,
                claveCliente.Value,
                campaignKeys,
                request.IdSubCartera,
                request.UnidadNegocio,
                request.IdSupervisor,
                request.FechaDesde,
                request.FechaHasta)
            .Where(row => row.ClaveSupervisor.HasValue);

        var managementRows = await activity
            .GroupBy(row => row.ClaveSupervisor!.Value)
            .Select(group => new MetricasGestionSupervisor(
                group.Key,
                group.Max(row => row.NombreSupervisor) ?? string.Empty,
                group.Min(row => row.FechaCalendario),
                group.Max(row => row.FechaCalendario),
                group.Sum(row => (long)(row.EventosGestion ?? 0)),
                group.Sum(row => row.MontoRecuperado ?? 0m),
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        if (managementRows.Count == 0)
        {
            return [];
        }

        var effectiveDateFrom = managementRows.Min(row => row.FechaDesde);
        var effectiveDateTo = managementRows.Max(row => row.FechaHasta);

        var advisorCountBySupervisor = await activity
            .Select(row => new
            {
                ClaveSupervisor = row.ClaveSupervisor!.Value,
                row.ClaveAsesor
            })
            .Distinct()
            .GroupBy(row => row.ClaveSupervisor)
            .Select(group => new
            {
                ClaveSupervisor = group.Key,
                CantidadAsesores = group.LongCount()
            })
            .ToDictionaryAsync(
                row => row.ClaveSupervisor,
                row => row.CantidadAsesores,
                cancellationToken);

        var contactRows = await ConstruirConsultaContacto(
                context,
                claveCliente.Value,
                campaignKeys,
                request.IdSubCartera,
                request.UnidadNegocio,
                request.IdSupervisor,
                effectiveDateFrom,
                effectiveDateTo)
            .Where(row => row.ClaveSupervisor.HasValue)
            .GroupBy(row => new
            {
                ClaveSupervisor = row.ClaveSupervisor!.Value,
                row.ClaveCampana,
                row.ClaveCartera,
                row.IdDeudorOrigen
            })
            .Select(group => new MetricasContactoDeudor(
                group.Key.ClaveSupervisor,
                group.Any(row => row.TuvoContactoDirecto),
                group.Any(row => row.TuvoContactoIndirecto),
                group.Any(row => row.TuvoSinContacto),
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var promiseRows = await ConstruirConsultaPromesa(
                context,
                claveCliente.Value,
                campaignKeys,
                request.IdSubCartera,
                request.UnidadNegocio,
                request.IdSupervisor,
                effectiveDateFrom,
                effectiveDateTo)
            .Where(row => row.ClaveSupervisor.HasValue && row.EsPromesaValida)
            .GroupBy(row => new
            {
                ClaveSupervisor = row.ClaveSupervisor!.Value,
                row.ClaveCampana,
                row.ClaveCartera,
                row.IdDeudorOrigen
            })
            .Select(group => new MetricasPromesaDeudor(
                group.Key.ClaveSupervisor,
                group.LongCount(),
                group.Sum(row => EstadosCumplidos.Contains(row.CodigoEstado)
                    ? row.MontoPagado ?? 0m
                    : 0m),
                group.Sum(row => EstadosCumplimiento.Contains(row.CodigoEstado)
                    ? row.MontoPromesa ?? 0m
                    : 0m),
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var paymentRows = await ConstruirConsultaPago(
                context,
                claveCliente.Value,
                campaignKeys,
                request.IdSubCartera,
                request.UnidadNegocio,
                request.IdSupervisor,
                effectiveDateFrom,
                effectiveDateTo)
            .Where(row => row.ClaveSupervisor.HasValue)
            .GroupBy(row => new
            {
                ClaveSupervisor = row.ClaveSupervisor!.Value,
                row.ClaveCampana,
                row.ClaveCartera,
                row.IdDeudorOrigen
            })
            .Select(group => new MetricasPagoDeudor(
                group.Key.ClaveSupervisor,
                group.Max(row => row.FechaCarga)))
            .ToListAsync(cancellationToken);

        var contactBySupervisor = contactRows
            .GroupBy(row => row.ActorKey)
            .ToDictionary(
                group => group.Key,
                group => new MetricasContacto(
                    group.LongCount(),
                    group.LongCount(row => row.TuvoContactoDirecto),
                    group.LongCount(row =>
                        row.TuvoContactoDirecto
                        || row.TuvoContactoIndirecto
                        || row.TuvoSinContacto),
                    MaximoNullable(group.Select(row => row.FechaCarga))));

        var promiseBySupervisor = promiseRows
            .GroupBy(row => row.ActorKey)
            .ToDictionary(
                group => group.Key,
                group => new MetricasPromesa(
                    group.Sum(row => row.CantidadPromesas),
                    group.LongCount(),
                    group.Sum(row => row.FulfillmentPaidAmount),
                    group.Sum(row => row.FulfillmentPromiseAmount),
                    MaximoNullable(group.Select(row => row.FechaCarga))));

        var paymentBySupervisor = paymentRows
            .GroupBy(row => row.ActorKey)
            .ToDictionary(
                group => group.Key,
                group => new MetricasPago(
                    group.LongCount(),
                    MaximoNullable(group.Select(row => row.FechaCarga))));

        return managementRows
            .Select(management =>
            {
                contactBySupervisor.TryGetValue(management.ClaveSupervisor, out var contact);
                promiseBySupervisor.TryGetValue(management.ClaveSupervisor, out var promise);
                paymentBySupervisor.TryGetValue(management.ClaveSupervisor, out var payment);

                return new RendimientoSupervisorCarteraDbFila
                {
                    IdSupervisor = management.ClaveSupervisor,
                    NombreSupervisor = management.NombreSupervisor,
                    FechaDesde = effectiveDateFrom,
                    FechaHasta = effectiveDateTo,
                    CantidadAsesores = advisorCountBySupervisor.GetValueOrDefault(
                        management.ClaveSupervisor),
                    CantidadGestiones = management.CantidadGestiones,
                    CantidadDeudoresGestionados = contact?.ManagedDebtors ?? 0,
                    TasaContactoDirecto = Dividir(
                        contact?.DirectContactClients ?? 0,
                        contact?.ClassifiableClients ?? 0),
                    TasaCierre = Dividir(
                        promise?.ClientesPromesaValida ?? 0,
                        contact?.DirectContactClients ?? 0),
                    CantidadPromesas = promise?.CantidadPromesas ?? 0,
                    TasaCumplimientoPromesa = Dividir(
                        promise?.FulfillmentPaidAmount ?? 0m,
                        promise?.FulfillmentPromiseAmount ?? 0m),
                    CantidadPagos = payment?.CantidadPagos ?? 0,
                    MontoRecuperadoAtribuible = Redondear(management.MontoRecuperado, 4),
                    FechaActualizacionUtc = MaximoNullable(
                    [
                        management.FechaCarga,
                        contact?.FechaCarga,
                        promise?.FechaCarga,
                        payment?.FechaCarga
                    ])
                };
            })
            .OrderBy(row => row.NombreSupervisor, StringComparer.Ordinal)
            .ThenBy(row => row.IdSupervisor)
            .ToArray();
    }

    private static async Task<int?> ResolverClaveClienteAsync(
        AnaliticaDbContext context,
        int idClienteCrm,
        string? unidadNegocio,
        CancellationToken cancellationToken)
    {
        var claveCliente = await context.ClientesAnalitica
            .AsNoTracking()
            .Where(client => client.IdClienteCrm == idClienteCrm)
            .Select(client => (int?)client.ClaveCliente)
            .SingleOrDefaultAsync(cancellationToken);

        if (!claveCliente.HasValue)
        {
            return null;
        }

        var availableBusinessUnits = await (
            from fact in context.HechosDiariosCarteraAnalitica.AsNoTracking()
            join portfolio in context.CarterasAnalitica.AsNoTracking()
                on fact.ClaveCartera equals portfolio.ClaveCartera
            where fact.ClaveCliente == claveCliente.Value
            select portfolio.UnidadNegocioOrigen)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (unidadNegocio is null)
        {
            var distinctScopes = availableBusinessUnits
                .Select(NormalizarUnidadNegocio)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(2)
                .Count();

            return distinctScopes > 1 ? null : claveCliente;
        }

        return availableBusinessUnits.Any(unit =>
            string.Equals(unit, unidadNegocio, StringComparison.Ordinal))
            ? claveCliente
            : null;
    }

    private static Task<int[]> ObtenerClavesCampanaAsync(
        AnaliticaDbContext context,
        int claveCliente,
        string? codigoCampana,
        CancellationToken cancellationToken) =>
        context.CampanasAnalitica
            .AsNoTracking()
            .Where(campana =>
                campana.ClaveCliente == claveCliente
                && (codigoCampana == null || campana.CodigoCampana == codigoCampana))
            .Select(campana => campana.ClaveCampana)
            .ToArrayAsync(cancellationToken);

    private static IQueryable<AtribucionDiariaSupervisorAsesorAnalitica> ConstruirConsultaActividad(
        AnaliticaDbContext context,
        int claveCliente,
        int[] campaignKeys,
        long? idSubCartera,
        string? unidadNegocio,
        int? idSupervisor,
        DateOnly? fechaDesde,
        DateOnly? fechaHasta)
    {
        var query = context.AtribucionesDiariasSupervisorAsesorAnalitica
            .AsNoTracking()
            .Where(row =>
                row.ClaveCliente == claveCliente
                && campaignKeys.Contains(row.ClaveCampana));

        if (idSubCartera.HasValue)
        {
            query = query.Where(row => row.ClaveCartera == idSubCartera.Value);
        }

        if (unidadNegocio is not null)
        {
            query = query.Where(row => context.CarterasAnalitica.Any(portfolio =>
                portfolio.ClaveCartera == row.ClaveCartera
                && portfolio.UnidadNegocioOrigen == unidadNegocio));
        }

        if (idSupervisor.HasValue)
        {
            query = query.Where(row => row.ClaveSupervisor == idSupervisor.Value);
        }

        if (fechaDesde.HasValue)
        {
            var from = fechaDesde.Value.ToDateTime(TimeOnly.MinValue);
            query = query.Where(row => row.FechaCalendario >= from);
        }

        if (fechaHasta.HasValue)
        {
            var to = fechaHasta.Value.ToDateTime(TimeOnly.MinValue);
            query = query.Where(row => row.FechaCalendario <= to);
        }

        return query;
    }

    private static IQueryable<ContactoDiarioDeudorSupervisorAnalitica> ConstruirConsultaContacto(
        AnaliticaDbContext context,
        int claveCliente,
        int[] campaignKeys,
        long? idSubCartera,
        string? unidadNegocio,
        int? idSupervisor,
        DateTime fechaDesde,
        DateTime fechaHasta)
    {
        var query = context.ContactoDiarioDeudorSupervisorAnalitica
            .AsNoTracking()
            .Where(row =>
                row.ClaveCliente == claveCliente
                && campaignKeys.Contains(row.ClaveCampana)
                && row.FechaCalendario >= fechaDesde
                && row.FechaCalendario <= fechaHasta);

        if (idSubCartera.HasValue)
        {
            query = query.Where(row => row.ClaveCartera == idSubCartera.Value);
        }

        if (unidadNegocio is not null)
        {
            query = query.Where(row => context.CarterasAnalitica.Any(portfolio =>
                portfolio.ClaveCartera == row.ClaveCartera
                && portfolio.UnidadNegocioOrigen == unidadNegocio));
        }

        if (idSupervisor.HasValue)
        {
            query = query.Where(row => row.ClaveSupervisor == idSupervisor.Value);
        }

        return query;
    }

    private static IQueryable<PromesaOperativaSupervisorAnalitica> ConstruirConsultaPromesa(
        AnaliticaDbContext context,
        int claveCliente,
        int[] campaignKeys,
        long? idSubCartera,
        string? unidadNegocio,
        int? idSupervisor,
        DateTime fechaDesde,
        DateTime fechaHasta)
    {
        var dateToExclusive = fechaHasta.AddDays(1);
        var query = context.PromesaOperativaSupervisorAnalitica
            .AsNoTracking()
            .Where(row =>
                row.ClaveCliente == claveCliente
                && campaignKeys.Contains(row.ClaveCampana)
                && row.FechaGestion >= fechaDesde
                && row.FechaGestion < dateToExclusive);

        if (idSubCartera.HasValue)
        {
            query = query.Where(row => row.ClaveCartera == idSubCartera.Value);
        }

        if (unidadNegocio is not null)
        {
            query = query.Where(row => context.CarterasAnalitica.Any(portfolio =>
                portfolio.ClaveCartera == row.ClaveCartera
                && portfolio.UnidadNegocioOrigen == unidadNegocio));
        }

        if (idSupervisor.HasValue)
        {
            query = query.Where(row => row.ClaveSupervisor == idSupervisor.Value);
        }

        return query;
    }

    private static IQueryable<PagoDiarioDeudorSupervisorAnalitica> ConstruirConsultaPago(
        AnaliticaDbContext context,
        int claveCliente,
        int[] campaignKeys,
        long? idSubCartera,
        string? unidadNegocio,
        int? idSupervisor,
        DateTime fechaDesde,
        DateTime fechaHasta)
    {
        var query = context.PagoDiarioDeudorSupervisorAnalitica
            .AsNoTracking()
            .Where(row =>
                row.ClaveCliente == claveCliente
                && campaignKeys.Contains(row.ClaveCampana)
                && row.FechaCalendario >= fechaDesde
                && row.FechaCalendario <= fechaHasta);

        if (idSubCartera.HasValue)
        {
            query = query.Where(row => row.ClaveCartera == idSubCartera.Value);
        }

        if (unidadNegocio is not null)
        {
            query = query.Where(row => context.CarterasAnalitica.Any(portfolio =>
                portfolio.ClaveCartera == row.ClaveCartera
                && portfolio.UnidadNegocioOrigen == unidadNegocio));
        }

        if (idSupervisor.HasValue)
        {
            query = query.Where(row => row.ClaveSupervisor == idSupervisor.Value);
        }

        return query;
    }

    private static decimal? Dividir(long numerator, long denominator) =>
        denominator == 0
            ? null
            : Redondear((decimal)numerator / denominator, 6);

    private static decimal? Dividir(decimal numerator, decimal denominator) =>
        denominator == 0m
            ? null
            : Redondear(numerator / denominator, 6);

    private static decimal Redondear(decimal value, int decimals) =>
        decimal.Round(value, decimals, MidpointRounding.AwayFromZero);

    private static DateTime? MaximoNullable(IEnumerable<DateTime?> values) =>
        values.Where(value => value.HasValue)
            .Select(value => value!.Value)
            .DefaultIfEmpty()
            .Max() is var max && max != default
                ? max
                : null;

    private static string? NormalizarUnidadNegocio(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? NormalizarNombre(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private sealed record MetricasGestionAsesor(
        int ClaveAsesor,
        string NombreAsesor,
        DateTime FechaDesde,
        DateTime FechaHasta,
        long CantidadGestiones,
        decimal MontoRecuperado,
        DateTime? FechaCarga);

    private sealed record MetricasGestionSupervisor(
        int ClaveSupervisor,
        string NombreSupervisor,
        DateTime FechaDesde,
        DateTime FechaHasta,
        long CantidadGestiones,
        decimal MontoRecuperado,
        DateTime? FechaCarga);

    private sealed record MetricasContactoDeudor(
        int ActorKey,
        bool TuvoContactoDirecto,
        bool TuvoContactoIndirecto,
        bool TuvoSinContacto,
        DateTime? FechaCarga);

    private sealed record MetricasPromesaDeudor(
        int ActorKey,
        long CantidadPromesas,
        decimal FulfillmentPaidAmount,
        decimal FulfillmentPromiseAmount,
        DateTime? FechaCarga);

    private sealed record MetricasPagoDeudor(
        int ActorKey,
        DateTime? FechaCarga);

    private sealed record MetricasContacto(
        long ManagedDebtors,
        long DirectContactClients,
        long ClassifiableClients,
        DateTime? FechaCarga);

    private sealed record MetricasPromesa(
        long CantidadPromesas,
        long ClientesPromesaValida,
        decimal FulfillmentPaidAmount,
        decimal FulfillmentPromiseAmount,
        DateTime? FechaCarga);

    private sealed record MetricasPago(
        long CantidadPagos,
        DateTime? FechaCarga);

    private sealed record AsignacionSupervisor(
        int? ClaveSupervisor,
        string? NombreSupervisor);
}
