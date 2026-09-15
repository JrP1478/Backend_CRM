using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Services.Analitica;
using GesMgmt.Domain.Constants;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;
using Moq;

namespace GesMgmt.UnitTests.Analitica.Application;

public sealed class AccesoReporteClienteAnaliticaServiceTests
{
    private const string ValidEmbedUrl = "https://app.powerbi.com/view?r=publicacion-prueba";

    [Fact]
    public async Task ResolverAsync_GestionIntegralCobranza_OmiteGrupoOpcionPeroConservaGruposDeCartera()
    {
        const int idUsuario = 10;
        const int idGrupoActual = 20;
        const int idGrupoCartera = 30;
        const int idClienteAutorizado = 40;

        var accesoOpcion = new Mock<IAccesoOpcionAnaliticaService>(MockBehavior.Strict);
        var configuracion = new Mock<IConfiguracionReporteClienteAnaliticaService>(
            MockBehavior.Strict);
        var permisos = new Mock<ISisgesOpcionPermisoRepository>(MockBehavior.Strict);

        permisos
            .Setup(repository => repository.TienePermisoAsync(
                idUsuario,
                idGrupoActual,
                AnaliticaOpcionIds.GestionIntegralCobranza,
                SisgesOptionPermission.Consult,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        accesoOpcion
            .Setup(service => service.ResolverAsync(
                idUsuario,
                AnaliticaOpcionIds.GestionIntegralCobranza,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AnaliticaAccesoOpcionResult(
                Permitido: false,
                ModoAlcance: "GROUP",
                IdsGruposCoincidentes: [],
                IdsGruposUsuarioActivos: [idGrupoCartera]));

        configuracion
            .Setup(service => service.ResolverAsync(
                AnaliticaOpcionIds.GestionIntegralCobranza,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                CrearConfiguracion(
                    idClienteAutorizado,
                    "CARTERA AUTORIZADA",
                    idGrupoCartera),
                CrearConfiguracion(
                    41,
                    "CARTERA DE OTRO GRUPO",
                    99)
            ]);

        var service = new AccesoReporteClienteAnaliticaService(
            accesoOpcion.Object,
            configuracion.Object,
            permisos.Object);

        var result = await service.ResolverAsync(
            idUsuario,
            idGrupoActual,
            AnaliticaOpcionIds.GestionIntegralCobranza,
            CancellationToken.None);

        Assert.True(result.TieneAccesoOpcion);
        var client = Assert.Single(result.Clientes);
        Assert.Equal(idClienteAutorizado, client.IdCliente);
        Assert.Equal("CARTERA AUTORIZADA", client.Nombre);

        accesoOpcion.VerifyAll();
        configuracion.VerifyAll();
        permisos.VerifyAll();
    }

    [Fact]
    public async Task ResolverAsync_GestionIntegralCobranza_SinPermisoSisges_DenegaAntesDeConsultarAlcances()
    {
        const int idUsuario = 10;
        const int idGrupo = 20;

        var accesoOpcion = new Mock<IAccesoOpcionAnaliticaService>(MockBehavior.Strict);
        var configuracion = new Mock<IConfiguracionReporteClienteAnaliticaService>(
            MockBehavior.Strict);
        var permisos = new Mock<ISisgesOpcionPermisoRepository>(MockBehavior.Strict);

        permisos
            .Setup(repository => repository.TienePermisoAsync(
                idUsuario,
                idGrupo,
                AnaliticaOpcionIds.GestionIntegralCobranza,
                SisgesOptionPermission.Consult,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var service = new AccesoReporteClienteAnaliticaService(
            accesoOpcion.Object,
            configuracion.Object,
            permisos.Object);

        var result = await service.ResolverAsync(
            idUsuario,
            idGrupo,
            AnaliticaOpcionIds.GestionIntegralCobranza,
            CancellationToken.None);

        Assert.False(result.TieneAccesoOpcion);
        Assert.Empty(result.Clientes);

        accesoOpcion.VerifyNoOtherCalls();
        configuracion.VerifyNoOtherCalls();
        permisos.VerifyAll();
    }

    [Fact]
    public async Task ResolverAsync_OtraOpcion_ConservaDenegacionPorGrupoOpcion()
    {
        const int idUsuario = 10;
        const int idGrupo = 20;
        const int idOpcion = 40;

        var accesoOpcion = new Mock<IAccesoOpcionAnaliticaService>(MockBehavior.Strict);
        var configuracion = new Mock<IConfiguracionReporteClienteAnaliticaService>(
            MockBehavior.Strict);
        var permisos = new Mock<ISisgesOpcionPermisoRepository>(MockBehavior.Strict);

        accesoOpcion
            .Setup(service => service.ResolverAsync(
                idUsuario,
                idOpcion,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AnaliticaAccesoOpcionResult(
                Permitido: false,
                ModoAlcance: "GROUP",
                IdsGruposCoincidentes: [],
                IdsGruposUsuarioActivos: [idGrupo]));

        var service = new AccesoReporteClienteAnaliticaService(
            accesoOpcion.Object,
            configuracion.Object,
            permisos.Object);

        var result = await service.ResolverAsync(
            idUsuario,
            idGrupo,
            idOpcion,
            CancellationToken.None);

        Assert.False(result.TieneAccesoOpcion);
        Assert.Empty(result.Clientes);

        accesoOpcion.VerifyAll();
        configuracion.VerifyNoOtherCalls();
        permisos.VerifyNoOtherCalls();
    }

    private static AnaliticaConfiguracionReporteCliente CrearConfiguracion(
        int idCliente,
        string nombre,
        int idGrupo) =>
        new(
            IdCliente: idCliente,
            Nombre: nombre,
            EstaDisponible: true,
            GroupResolution: AnaliticaReporteClienteGrupoResolucion.Configurado,
            TieneConfiguracionGruposExplicita: true,
            IdsGrupos: [idGrupo],
            GruposCandidatos: [],
            UrlIncrustacion: ValidEmbedUrl,
            EstaLista: true);
}
