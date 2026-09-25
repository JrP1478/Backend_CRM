using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.UnitTests.Analitica.Portfolio;

public sealed class UnidadNegocioCarteraContratoTests
{
    [Fact]
    public void TryResolve_UsesCanonicalExplicitBusinessUnit()
    {
        var success = UnidadNegocioCarteraContrato.IntentarResolver(
            " cliente_a gobierno ",
            "CLIENTE_A ADMINISTRATIVO",
            ["CLIENTE_A ADMINISTRATIVO", "CLIENTE_A GOBIERNO"],
            out var selection,
            out var errors);

        Assert.True(success);
        Assert.Empty(errors);
        Assert.Equal("CLIENTE_A GOBIERNO", selection.SelectedBusinessUnit);
        Assert.False(selection.WasDefaulted);
        Assert.True(selection.TieneMultiplesUnidadesNegocio);
    }

    [Fact]
    public void TryResolve_OmittedSelectionUsesBackwardCompatibleDefault()
    {
        var success = UnidadNegocioCarteraContrato.IntentarResolver(
            null,
            " cliente_a administrativo ",
            ["CLIENTE_A GOBIERNO", "CLIENTE_A ADMINISTRATIVO"],
            out var selection,
            out var errors);

        Assert.True(success);
        Assert.Empty(errors);
        Assert.Equal("CLIENTE_A ADMINISTRATIVO", selection.SelectedBusinessUnit);
        Assert.True(selection.WasDefaulted);
        Assert.Equal(
            new[] { "CLIENTE_A ADMINISTRATIVO", "CLIENTE_A GOBIERNO" },
            selection.UnidadesNegocioDisponibles.ToArray());
    }

    [Fact]
    public void TryResolve_RejectsBusinessUnitOutsideAvailableData()
    {
        var success = UnidadNegocioCarteraContrato.IntentarResolver(
            "OTRA UNIDAD",
            "CLIENTE_A ADMINISTRATIVO",
            ["CLIENTE_A ADMINISTRATIVO", "CLIENTE_A GOBIERNO"],
            out var selection,
            out var errors);

        Assert.False(success);
        Assert.Null(selection.SelectedBusinessUnit);
        Assert.Contains("unidadNegocio", errors.Keys);
    }
}
