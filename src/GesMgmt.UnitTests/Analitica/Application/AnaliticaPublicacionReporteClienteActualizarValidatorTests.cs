using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.UnitTests.Analitica.Application;

public sealed class AnaliticaPublicacionReporteClienteActualizarValidatorTests
{
    [Fact]
    public void Validate_WhenOnlyUrlChanges_PreservesNullGroupIds()
    {
        var configuration = CreateConfiguration(
            AnaliticaReporteClienteGrupoResolucion.DetectadoAutomaticamente,
            false,
            [219]);

        var result = AnaliticaPublicacionReporteClienteActualizarValidator.Validar(
            [new ActualizarAnaliticaIncrustacionReporteClienteOpcion(
                178,
                "ADEX INSTITUTO",
                null,
                "https://app.powerbi.com/view?r=test")],
            [configuration]);

        Assert.Null(result.Error);
        var update = Assert.Single(result.Updates);
        Assert.Null(update.IdsGrupos);
        Assert.Equal("https://app.powerbi.com/view?r=test", update.UrlIncrustacion);
    }

    [Fact]
    public void Validate_WhenRequestedGroupIsOutsideClient_ReturnsValidationError()
    {
        var configuration = CreateConfiguration(
            AnaliticaReporteClienteGrupoResolucion.Configurado,
            true,
            [219]);

        var result = AnaliticaPublicacionReporteClienteActualizarValidator.Validar(
            [new ActualizarAnaliticaIncrustacionReporteClienteOpcion(
                178,
                "ADEX INSTITUTO",
                [999],
                "https://app.powerbi.com/view?r=test")],
            [configuration]);

        Assert.NotNull(result.Error);
        Assert.Equal("Grupo fuera del cliente", result.Error?.Titulo);
        Assert.Empty(result.Updates);
    }

    [Fact]
    public void Validate_WhenPublishToWebIsDisabled_RejectsPublicUrl()
    {
        var configuration = CreateConfiguration(
            AnaliticaReporteClienteGrupoResolucion.Configurado,
            true,
            [219]);

        var result = AnaliticaPublicacionReporteClienteActualizarValidator.Validar(
            [new ActualizarAnaliticaIncrustacionReporteClienteOpcion(
                178,
                "ADEX INSTITUTO",
                [219],
                "https://app.powerbi.com/view?r=test")],
            [configuration],
            allowPublishToWeb: false);

        Assert.NotNull(result.Error);
        Assert.Equal("Publicar en web deshabilitado", result.Error?.Titulo);
        Assert.Empty(result.Updates);
    }

    private static AnaliticaConfiguracionReporteCliente CreateConfiguration(
        string groupResolution,
        bool hasExplicitGroupConfiguration,
        IReadOnlyList<int> idsGrupos) =>
        new(
            178,
            "ADEX INSTITUTO",
            true,
            groupResolution,
            hasExplicitGroupConfiguration,
            idsGrupos,
            [new AnaliticaConfiguracionReporteClienteGrupo(219, "ADEX INSTITUTO [219]")],
            "https://app.powerbi.com/view?r=old",
            true);
}
