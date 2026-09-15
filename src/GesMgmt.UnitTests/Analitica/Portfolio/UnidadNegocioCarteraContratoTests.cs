using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.UnitTests.Analitica.Portfolio;

public sealed class UnidadNegocioCarteraContratoTests
{
    [Fact]
    public void TryResolve_UsesCanonicalExplicitBusinessUnit()
    {
        var success = UnidadNegocioCarteraContrato.IntentarResolver(
            " claro gobierno ",
            "CLARO ADMINISTRATIVO",
            ["CLARO ADMINISTRATIVO", "CLARO GOBIERNO"],
            out var selection,
            out var errors);

        Assert.True(success);
        Assert.Empty(errors);
        Assert.Equal("CLARO GOBIERNO", selection.SelectedBusinessUnit);
        Assert.False(selection.WasDefaulted);
        Assert.True(selection.TieneMultiplesUnidadesNegocio);
    }

    [Fact]
    public void TryResolve_OmittedSelectionUsesBackwardCompatibleDefault()
    {
        var success = UnidadNegocioCarteraContrato.IntentarResolver(
            null,
            " claro administrativo ",
            ["CLARO GOBIERNO", "CLARO ADMINISTRATIVO"],
            out var selection,
            out var errors);

        Assert.True(success);
        Assert.Empty(errors);
        Assert.Equal("CLARO ADMINISTRATIVO", selection.SelectedBusinessUnit);
        Assert.True(selection.WasDefaulted);
        Assert.Equal(
            new[] { "CLARO ADMINISTRATIVO", "CLARO GOBIERNO" },
            selection.UnidadesNegocioDisponibles.ToArray());
    }

    [Fact]
    public void TryResolve_RejectsBusinessUnitOutsideAvailableData()
    {
        var success = UnidadNegocioCarteraContrato.IntentarResolver(
            "OTRA UNIDAD",
            "CLARO ADMINISTRATIVO",
            ["CLARO ADMINISTRATIVO", "CLARO GOBIERNO"],
            out var selection,
            out var errors);

        Assert.False(success);
        Assert.Null(selection.SelectedBusinessUnit);
        Assert.Contains("unidadNegocio", errors.Keys);
    }
}
