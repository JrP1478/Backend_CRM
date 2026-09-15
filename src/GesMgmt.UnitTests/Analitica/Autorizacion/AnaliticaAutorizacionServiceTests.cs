using GesMgmt.Application.Services.Analitica;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.UnitTests.Analitica.Autorizacion;

public sealed class AnaliticaAutorizacionServiceTests
{
    [Fact]
    public async Task CanAccessAdministrationAsync_UsesMaintainModuleAndCurrentGroup()
    {
        var repository = new RecordingPermissionRepository(result: true);
        var service = new AutorizacionAnaliticaService(repository);

        var result = await service.PuedeAccederAdministracionAsync(
            16068,
            156,
            SisgesOptionPermission.Edit,
            CancellationToken.None);

        Assert.True(result.Permitido);
        Assert.Equal(16068, repository.IdUsuario);
        Assert.Equal(156, repository.IdGrupo);
        Assert.Equal(SisgesCodigosOpcion.MantenerModulo, repository.CodigoOpcion);
        Assert.Equal(SisgesOptionPermission.Edit, repository.Permiso);
    }

    [Fact]
    public async Task CanAccessAdministrationAsync_DeniesWhenSisgesPermissionIsMissing()
    {
        var repository = new RecordingPermissionRepository(result: false);
        var service = new AutorizacionAnaliticaService(repository);

        var result = await service.PuedeAccederAdministracionAsync(
            16068,
            null,
            SisgesOptionPermission.Consult,
            CancellationToken.None);

        Assert.False(result.Permitido);
        Assert.Contains("Mantener módulo", result.Reason);
    }

    private sealed class RecordingPermissionRepository(bool result)
        : ISisgesOpcionPermisoRepository
    {
        public int IdUsuario { get; private set; }
        public int? IdGrupo { get; private set; }
        public string? CodigoOpcion { get; private set; }
        public SisgesOptionPermission Permiso { get; private set; }

        public Task<bool> TienePermisoAsync(
            int idUsuario,
            int? idGrupo,
            string codigoOpcion,
            SisgesOptionPermission permiso,
            CancellationToken cancellationToken)
        {
            IdUsuario = idUsuario;
            IdGrupo = idGrupo;
            CodigoOpcion = codigoOpcion;
            Permiso = permiso;
            return Task.FromResult(result);
        }

        public Task<bool> TienePermisoAsync(
            int idUsuario,
            int? idGrupo,
            int idOpcion,
            SisgesOptionPermission permiso,
            CancellationToken cancellationToken) =>
            Task.FromResult(result);
    }
}
