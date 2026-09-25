using System.Data;
using System.Text.Json;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class AnaliticaAlcanceClienteOpcionRepository(
    GesMgmt.Infraestructure.Persistence.AnaliticaDbContext context)
    : IAnaliticaAlcanceClienteOpcionRepository
{
    public async Task<IReadOnlyList<int>> ObtenerIdsClientesAsync(
        int idOpcion,
        CancellationToken cancellationToken) =>
        await context.AlcancesOpcionClienteAnalitica
            .AsNoTracking()
            .Where(scope =>
                scope.IdOpcion == idOpcion &&
                scope.EsActivo)
            .OrderBy(scope => scope.IdClienteCrm)
            .Select(scope => scope.IdClienteCrm)
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyList<int>> ObtenerIdsClientesAutorizadosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken) =>
        await ObtenerAlcancesAutorizados(idUsuario, idOpcion)
            .OrderBy(scope => scope.IdClienteCrm)
            .Select(scope => scope.IdClienteCrm)
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyList<AnaliticaAlcanceClienteAutorizadoEntrada>> ObtenerClientesAutorizadosAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken)
    {
        var rows = await (
            from scope in ObtenerAlcancesAutorizados(idUsuario, idOpcion)
            join client in context.ClientesAnalitica.AsNoTracking()
                on scope.IdClienteCrm equals client.IdClienteCrm into clientes
            from client in clientes.DefaultIfEmpty()
            orderby scope.IdClienteCrm
            select new
            {
                scope.IdClienteCrm,
                client.NombreCliente,
                client.CodigoCliente
            })
            .ToArrayAsync(cancellationToken);

        return rows
            .Select(row => new AnaliticaAlcanceClienteAutorizadoEntrada
            {
                IdClienteCrm = row.IdClienteCrm,
                Nombre = ResolverNombreCliente(
                    row.IdClienteCrm,
                    row.NombreCliente,
                    row.CodigoCliente)
            })
            .ToArray();
    }

    public Task<bool> EsClienteAutorizadoAsync(
        int idUsuario,
        int idOpcion,
        int idCliente,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 || idOpcion <= 0 || idCliente <= 0)
        {
            return Task.FromResult(false);
        }

        return ObtenerAlcancesAutorizados(idUsuario, idOpcion)
            .AnyAsync(
                scope => scope.IdClienteCrm == idCliente,
                cancellationToken);
    }

    public async Task<IReadOnlyList<AlcanceOpcionClienteAnalitica>> ObtenerAlcancesActivosAsync(
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken)
    {
        var normalizedOptionIds = idsOpciones
            .Where(idOpcion => idOpcion > 0)
            .Distinct()
            .OrderBy(idOpcion => idOpcion)
            .ToArray();

        if (normalizedOptionIds.Length == 0)
        {
            return Array.Empty<AlcanceOpcionClienteAnalitica>();
        }

        return await (
            from scope in context.AlcancesOpcionClienteAnalitica.AsNoTracking()
            join option in context.ConfiguracionesOpcionAnalitica.AsNoTracking()
                on scope.IdOpcion equals option.IdOpcion
            where normalizedOptionIds.Contains(scope.IdOpcion)
                && scope.EsActivo
                && option.EsActivo
            orderby scope.IdOpcion, scope.IdClienteCrm
            select new AlcanceOpcionClienteAnalitica
            {
                IdOpcion = scope.IdOpcion,
                IdClienteCrm = scope.IdClienteCrm,
                EsActivo = true
            })
            .ToArrayAsync(cancellationToken);
    }

    public async Task ReemplazarAsync(
        int idOpcion,
        IReadOnlyCollection<int> previousClientIds,
        IReadOnlyCollection<int> idsClientes,
        int? idUsuario,
        CancellationToken cancellationToken)
    {
        var normalizedPreviousClientIds = Normalizar(previousClientIds);
        var normalizedClientIds = Normalizar(idsClientes);
        var requestedClientIds = normalizedClientIds.ToHashSet();
        var now = DateTime.UtcNow;

        await using var transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        var existingScopes = await context.AlcancesOpcionClienteAnalitica
            .Where(scope => scope.IdOpcion == idOpcion)
            .ToListAsync(cancellationToken);

        foreach (var scope in existingScopes)
        {
            if (requestedClientIds.Contains(scope.IdClienteCrm))
            {
                scope.EsActivo = true;
                scope.ActualizadoPor = idUsuario;
                scope.FechaActualizacion = now;
                requestedClientIds.Remove(scope.IdClienteCrm);
                continue;
            }

            if (!scope.EsActivo)
            {
                continue;
            }

            scope.EsActivo = false;
            scope.ActualizadoPor = idUsuario;
            scope.FechaActualizacion = now;
        }

        foreach (var idCliente in requestedClientIds.OrderBy(idCliente => idCliente))
        {
            await context.AlcancesOpcionClienteAnalitica.AddAsync(
                new AlcanceOpcionClienteAnalitica
                {
                    IdOpcion = idOpcion,
                    IdClienteCrm = idCliente,
                    EsActivo = true,
                    CreadoPor = idUsuario,
                    FechaCreacion = now
                },
                cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        var previousClientIdsJson = JsonSerializer.Serialize(normalizedPreviousClientIds);
        var newClientIdsJson = JsonSerializer.Serialize(normalizedClientIds);

        await context.Database.ExecuteSqlInterpolatedAsync(
            $"""
            INSERT INTO acceso_analitica.auditoria_alcance_opcion_cliente
            (
                id_opcion,
                ids_clientes_anteriores,
                ids_clientes_nuevos,
                creado_por,
                fecha_creacion
            )
            VALUES
            (
                {idOpcion},
                {previousClientIdsJson},
                {newClientIdsJson},
                {idUsuario},
                {now}
            );
            """,
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    private IQueryable<AlcanceOpcionClienteAnalitica> ObtenerAlcancesAutorizados(
        int idUsuario,
        int idOpcion)
    {
        var query = context.AlcancesOpcionClienteAnalitica
            .AsNoTracking()
            .Where(scope =>
                scope.IdOpcion == idOpcion &&
                scope.EsActivo &&
                context.ConfiguracionesOpcionAnalitica.Any(option =>
                    option.IdOpcion == scope.IdOpcion &&
                    option.EsActivo));

        if (UsaAccesoClienteHeredado(idOpcion))
        {
            return query;
        }

        return query.Where(scope =>
            context.AlcancesUsuarioOpcionAnalitica.Any(userScope =>
                userScope.IdUsuario == idUsuario &&
                userScope.IdOpcion == scope.IdOpcion &&
                userScope.EsActivo));
    }

    private static string ResolverNombreCliente(
        int idCliente,
        string? nombreCliente,
        string? codigoCliente)
    {
        var normalizedName = nombreCliente?.Trim();

        if (!string.IsNullOrWhiteSpace(normalizedName))
        {
            return normalizedName;
        }

        var normalizedCode = codigoCliente?.Trim();

        return !string.IsNullOrWhiteSpace(normalizedCode)
            ? normalizedCode
            : $"Cartera {idCliente}";
    }

    // Centro de Control de Cartera is an operational module whose data scope is
    // inherited from CRM user/group/client assignments. Requiring a second
    // per-user row in Analítica duplicates authorization state and does not
    // scale as users are added or moved between grupos. Other options retain
    // the explicit user_option_scope behavior.
    private static bool UsaAccesoClienteHeredado(int idOpcion) =>
        idOpcion == AnaliticaOpcionIds.CentroControlCartera;

    private static int[] Normalizar(IReadOnlyCollection<int> idsClientes) =>
        idsClientes
            .Where(idCliente => idCliente > 0)
            .Distinct()
            .OrderBy(idCliente => idCliente)
            .ToArray();
}
