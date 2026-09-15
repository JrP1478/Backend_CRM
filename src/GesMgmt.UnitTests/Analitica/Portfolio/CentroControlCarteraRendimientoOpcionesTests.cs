using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

namespace GesMgmt.UnitTests.Analitica.Portfolio;

public sealed class CentroControlCarteraRendimientoOpcionesTests
{
    [Fact]
    public void Predeterminados_ConservanValoresOriginalesProteccionRecursos()
    {
        var options = new RendimientoCentroControlCarteraOptions();

        Assert.Equal(80, options.MaximoSolicitudesConcurrentes);
        Assert.Equal(240, options.LimiteCola);
        Assert.Equal(60, options.SegundosTiempoEsperaSolicitud);
        Assert.Equal(30, options.SegundosCacheDetalle);
        Assert.Equal(512, options.MaximoEntradasCacheDetalle);
        options.Validar();
    }

    [Theory]
    [InlineData(0, 240, 60, 30, 512)]
    [InlineData(513, 240, 60, 30, 512)]
    [InlineData(80, -1, 60, 30, 512)]
    [InlineData(80, 4097, 60, 30, 512)]
    [InlineData(80, 240, 0, 30, 512)]
    [InlineData(80, 240, 301, 30, 512)]
    [InlineData(80, 240, 60, -1, 512)]
    [InlineData(80, 240, 60, 301, 512)]
    [InlineData(80, 240, 60, 30, 0)]
    [InlineData(80, 240, 60, 30, 4097)]
    public void Validar_ValorInvalido_FallaInmediatamente(
        int maximoSolicitudesConcurrentes,
        int limiteCola,
        int segundosTiempoEsperaSolicitud,
        int segundosCacheDetalle,
        int maximoEntradasCacheDetalle)
    {
        var options = new RendimientoCentroControlCarteraOptions
        {
            MaximoSolicitudesConcurrentes = maximoSolicitudesConcurrentes,
            LimiteCola = limiteCola,
            SegundosTiempoEsperaSolicitud = segundosTiempoEsperaSolicitud,
            SegundosCacheDetalle = segundosCacheDetalle,
            MaximoEntradasCacheDetalle = maximoEntradasCacheDetalle
        };

        Assert.Throws<InvalidOperationException>(options.Validar);
    }
}
