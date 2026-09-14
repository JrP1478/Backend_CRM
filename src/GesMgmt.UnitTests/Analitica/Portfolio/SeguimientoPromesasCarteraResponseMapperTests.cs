using GesMgmt.Application.Services.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.UnitTests.Analitica.Portfolio;

public sealed class SeguimientoPromesasCarteraResponseMapperTests
{
    [Fact]
    public void Map_ExponeSeguimientoOperativoPorDeudor()
    {
        var contexto = new PromesasCarteraContexto(
            1,
            15,
            "2026-09",
            "Septiembre 2026");

        var resultado = new SeguimientoPromesasCarteraConsultaResult(
            new DateOnly(2026, 9, 14),
            new SeguimientoPromesasCarteraResumenDbFila
            {
                CantidadPromesas = 1,
                MontoPromesa = 184.99m,
                MontoPagado = 0m,
                MontoPendiente = 184.99m,
                FechaCorte = new DateTime(2026, 9, 14),
                FechaActualizacionUtc = new DateTime(2026, 9, 14, 16, 20, 20)
            },
            [
                new SeguimientoPromesasCarteraEstadoDbFila
                {
                    ClaveEstado = "pendiente",
                    CantidadPromesas = 1,
                    MontoPromesa = 184.99m,
                    MontoPagado = 0m,
                    MontoPendiente = 184.99m
                }
            ],
            [
                new SeguimientoPromesaCarteraDbFila
                {
                    IdPromesa = 4951,
                    IdDeudor = 18330126,
                    NombreDeudor = "INVERSIONES METCON SAC",
                    FechaVencimiento = new DateTime(2026, 9, 14),
                    MontoPromesa = 184.99m,
                    MontoPagado = 0m,
                    MontoPendiente = 184.99m,
                    ClaveEstado = "pendiente",
                    Gestionado = true,
                    CantidadGestiones = 2,
                    CantidadLlamadas = 2,
                    ClaveContacto = "directo",
                    ConfirmoPago = true,
                    FechaUltimaGestion = new DateTime(2026, 9, 14, 9, 3, 10),
                    IdAsesor = 8,
                    NombreAsesor = "MIGUEL SOLANGE JHAMILE"
                }
            ],
            PromesasCarteraPaginacion.Crear(1, 20, 1));

        var response = SeguimientoPromesasCarteraResponseMapper.Map(contexto, resultado);

        var item = Assert.Single(response.Elementos);
        Assert.Equal("INVERSIONES METCON SAC", item.NombreDeudor);
        Assert.True(item.Gestionado);
        Assert.Equal(2, item.CantidadGestiones);
        Assert.Equal(2, item.CantidadLlamadas);
        Assert.Equal("directo", item.ClaveContacto);
        Assert.Equal("Contacto directo", item.EtiquetaContacto);
        Assert.True(item.ConfirmoPago is true);
        Assert.Equal(new DateTime(2026, 9, 14, 9, 3, 10), item.FechaUltimaGestion);
        Assert.Equal("Pendiente", Assert.Single(response.Estado).Etiqueta);
    }
}
