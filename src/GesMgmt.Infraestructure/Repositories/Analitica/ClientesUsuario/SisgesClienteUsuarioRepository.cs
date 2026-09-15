using GesMgmt.Domain.Interfaces.Analitica;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

internal sealed class SisgesClienteUsuarioRepository(AvalDbContext context)
    : ISisgesClienteUsuarioRepository
{
    public async Task<IReadOnlyList<int>> ObtenerIdsClientesActivosAsync(
        int idUsuario,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0)
        {
            return Array.Empty<int>();
        }

        return await context.av_UGrupos
            .AsNoTracking()
            .Where(userGroup =>
                userGroup.nId_Usuario == idUsuario &&
                userGroup.bEstado == true &&
                userGroup.bActivo == true &&
                userGroup.av_Usuario.bEstado &&
                userGroup.av_Grupo.bEstado == true &&
                userGroup.av_Grupo.nid_cliente.HasValue &&
                userGroup.av_Grupo.nid_cliente.Value > 0)
            .Select(userGroup => userGroup.av_Grupo.nid_cliente!.Value)
            .Distinct()
            .OrderBy(idCliente => idCliente)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> EsClienteActivoAsync(
        int idUsuario,
        int idCliente,
        CancellationToken cancellationToken)
    {
        if (idUsuario <= 0 || idCliente <= 0)
        {
            return Task.FromResult(false);
        }

        return context.av_UGrupos
            .AsNoTracking()
            .AnyAsync(
                userGroup =>
                    userGroup.nId_Usuario == idUsuario &&
                    userGroup.bEstado == true &&
                    userGroup.bActivo == true &&
                    userGroup.av_Usuario.bEstado &&
                    userGroup.av_Grupo.bEstado == true &&
                    userGroup.av_Grupo.nid_cliente == idCliente,
                cancellationToken);
    }
}
