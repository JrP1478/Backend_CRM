using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class SisgesGrupoClienteRepository(AvalDbContext context)
    : ISisgesGrupoClienteRepository
{
    public async Task<IReadOnlyList<SisgesGrupoCliente>> ObtenerTodosGruposActivosAsync(
        CancellationToken cancellationToken) =>
        await context.av_Grupos
            .AsNoTracking()
            .Where(group =>
                group.bEstado == true &&
                group.nid_cliente.HasValue &&
                group.nid_cliente.Value > 0)
            .Select(group => new SisgesGrupoCliente
            {
                IdGrupo = group.nId_Grupo,
                IdCliente = group.nid_cliente!.Value,
                NombreGrupo = (group.cNombre_Grupo ?? string.Empty).Trim()
            })
            .OrderBy(group => group.NombreGrupo)
            .ThenBy(group => group.IdGrupo)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<SisgesGrupoCliente>> ObtenerGruposActivosAsync(
        IReadOnlyCollection<int> idsClientes,
        CancellationToken cancellationToken)
    {
        var normalizedClientIds = idsClientes
            .Where(idCliente => idCliente > 0)
            .Distinct()
            .OrderBy(idCliente => idCliente)
            .ToArray();

        if (normalizedClientIds.Length == 0)
        {
            return Array.Empty<SisgesGrupoCliente>();
        }

        return await context.av_Grupos
            .AsNoTracking()
            .Where(group =>
                group.bEstado == true &&
                group.nid_cliente.HasValue &&
                group.nid_cliente.Value > 0 &&
                normalizedClientIds.Contains(group.nid_cliente.Value))
            .OrderBy(group => group.nid_cliente)
            .ThenBy(group => group.nId_Grupo)
            .Select(group => new SisgesGrupoCliente
            {
                IdGrupo = group.nId_Grupo,
                IdCliente = group.nid_cliente!.Value,
                NombreGrupo = (group.cNombre_Grupo ?? string.Empty).Trim()
            })
            .ToListAsync(cancellationToken);
    }
}
