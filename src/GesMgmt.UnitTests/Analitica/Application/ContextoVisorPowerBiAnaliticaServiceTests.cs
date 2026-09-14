using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Services.Analitica;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Interfaces.Analitica;
using Moq;

namespace GesMgmt.UnitTests.Analitica.Application;

public sealed class ContextoVisorPowerBiAnaliticaServiceTests
{
    [Fact]
    public async Task ResolverAsync_ReporteDirecto_NoConsultaAlcancesDeCliente()
    {
        const int idUsuario = 10;
        const int idGrupo = 20;
        const int idOpcion = 40;

        var accesoOpcion = new Mock<IAccesoOpcionAnaliticaService>(MockBehavior.Strict);
        var configuracion = new Mock<IConfiguracionReporteClienteAnaliticaService>(
            MockBehavior.Strict);
        var permisos = new Mock<ISisgesOpcionPermisoRepository>(MockBehavior.Strict);

        permisos
            .Setup(repository => repository.TienePermisoAsync(
                idUsuario,
                idGrupo,
                idOpcion,
                SisgesOptionPermission.Consult,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        configuracion
            .Setup(service => service.RequiereSeleccionClienteAsync(
                idOpcion,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new ContextoVisorPowerBiAnaliticaService(
            accesoOpcion.Object,
            configuracion.Object,
            permisos.Object);

        var result = await service.ResolverAsync(
            idUsuario,
            idGrupo,
            idOpcion,
            seleccion: null,
            CancellationToken.None);

        Assert.True(result.Permitido);
        Assert.False(result.RequiereSeleccionCliente);
        Assert.Equal("NOT_REQUIRED", result.EstadoSeleccionCliente);
        Assert.Null(result.ClienteSeleccionado);
        Assert.Null(result.UrlIncrustacion);

        accesoOpcion.VerifyNoOtherCalls();
        configuracion.VerifyAll();
        permisos.VerifyAll();
    }
}
