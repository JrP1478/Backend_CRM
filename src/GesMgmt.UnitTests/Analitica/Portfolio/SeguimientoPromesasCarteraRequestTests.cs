using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.UnitTests.Analitica.Portfolio;

public sealed class SeguimientoPromesasCarteraRequestTests
{
    [Fact]
    public void IntentarCrear_AceptaFechaIsoYEstadoHistorico()
    {
        var ok = SeguimientoPromesasCarteraRequest.IntentarCrear(
            "2026-09",
            null,
            "2026-09-13",
            "1",
            "20",
            "incumplida",
            "montoPendiente",
            "desc",
            out var request,
            out var errors);

        Assert.True(ok);
        Assert.Empty(errors);
        Assert.NotNull(request);
        Assert.Equal(new DateOnly(2026, 9, 13), request!.FechaVencimiento);
        Assert.Equal("incumplida", request.Estado);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("14/09/2026")]
    [InlineData("2026-13-40")]
    public void IntentarCrear_RechazaFechaAusenteONoIso(string? fecha)
    {
        var ok = SeguimientoPromesasCarteraRequest.IntentarCrear(
            "2026-09",
            null,
            fecha,
            null,
            null,
            null,
            null,
            null,
            out var request,
            out var errors);

        Assert.False(ok);
        Assert.Null(request);
        Assert.Contains("fechaVencimiento", errors.Keys);
    }
}
