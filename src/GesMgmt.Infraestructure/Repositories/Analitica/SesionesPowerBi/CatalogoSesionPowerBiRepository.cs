using GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;
using GesMgmt.Domain.Interfaces.Analitica.SesionesPowerBi;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories.Analitica.SesionesPowerBi;

internal sealed class CatalogoSesionPowerBiRepository(CrmDbContext context)
    : ICatalogoSesionPowerBiRepository
{
    public async Task<SnapshotIdentidadReportePowerBi?> ObtenerSnapshotAsync(
        int idUsuario,
        int idOpcionReporte,
        CancellationToken cancellationToken)
    {
        var usuario = await context.Crm_Usuarios
            .AsNoTracking()
            .Where(row => row.nId_Usuario == idUsuario && row.bEstado)
            .Select(row => new
            {
                row.nId_Usuario,
                row.cUsr_Login,
                row.cUsr_Nombres,
                row.cUsr_ApePat,
                row.cUsr_ApeMat
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (usuario is null)
        {
            return null;
        }

        var reporte = await context.Crm_Opcions
            .AsNoTracking()
            .Where(row => row.nId_Opcion == idOpcionReporte && row.bEstado)
            .Select(row => new
            {
                row.nId_Opcion,
                row.sNombreOpcion
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (reporte is null)
        {
            return null;
        }

        var login = usuario.cUsr_Login?.Trim();
        if (string.IsNullOrWhiteSpace(login))
        {
            login = $"USUARIO-{idUsuario}";
        }

        var nombreUsuario = string.Join(
                " ",
                new[]
                {
                    usuario.cUsr_Nombres,
                    usuario.cUsr_ApePat,
                    usuario.cUsr_ApeMat
                }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!.Trim()))
            .Trim();

        if (nombreUsuario.Length == 0)
        {
            nombreUsuario = login;
        }

        var nombreReporte = reporte.sNombreOpcion?.Trim();
        if (string.IsNullOrWhiteSpace(nombreReporte))
        {
            nombreReporte = $"OPCION-{idOpcionReporte}";
        }

        return new SnapshotIdentidadReportePowerBi(
            usuario.nId_Usuario,
            login,
            nombreUsuario,
            reporte.nId_Opcion,
            nombreReporte);
    }
}
