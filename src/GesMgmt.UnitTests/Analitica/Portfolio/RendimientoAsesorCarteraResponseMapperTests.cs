using GesMgmt.Application.Services.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.UnitTests.Analitica.Portfolio;

public sealed class RendimientoAsesorCarteraResponseMapperTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Map_UsesExplicitMissingDataLabel_WhenAdvisorNameIsBlank(string nombreAsesor)
    {
        RendimientoAsesorCarteraDbFila[] rows =
        [
            CreateRow(idAsesor: 42, nombreAsesor: nombreAsesor)
        ];

        var response = RendimientoAsesorCarteraResponseMapper.Map(rows);

        var advisor = Assert.Single(response.Asesores);
        Assert.Equal(42, advisor.IdAsesor);
        Assert.Equal("Sin nombre (ID 42)", advisor.NombreAsesor);
    }

    [Fact]
    public void Map_UsesExplicitMissingDataLabel_WhenDatabaseReturnsNullAdvisorName()
    {
        RendimientoAsesorCarteraDbFila[] rows =
        [
            CreateRow(idAsesor: 73, nombreAsesor: null!)
        ];

        var response = RendimientoAsesorCarteraResponseMapper.Map(rows);

        Assert.Equal("Sin nombre (ID 73)", Assert.Single(response.Asesores).NombreAsesor);
    }

    [Fact]
    public void Map_TrimsNames_AndNormalizesBlankOptionalSupervisorNameToNull()
    {
        RendimientoAsesorCarteraDbFila[] rows =
        [
            CreateRow(
                idAsesor: 9,
                nombreAsesor: "  PEREZ JUAN  ",
                idSupervisorPeriodo: 7,
                nombreSupervisorPeriodo: "  SUPERVISORA DEL PERIODO  ",
                idSupervisorActual: 3,
                nombreSupervisorActual: "   ")
        ];

        var advisor = Assert.Single(RendimientoAsesorCarteraResponseMapper.Map(rows).Asesores);

        Assert.Equal("PEREZ JUAN", advisor.NombreAsesor);
        Assert.Equal(7, advisor.IdSupervisorPeriodo);
        Assert.Equal("SUPERVISORA DEL PERIODO", advisor.NombreSupervisorPeriodo);
        Assert.Equal(3, advisor.IdSupervisorActual);
        Assert.Null(advisor.NombreSupervisorActual);
        Assert.Equal(7, advisor.CantidadDeudoresGestionados);
    }

    private static RendimientoAsesorCarteraDbFila CreateRow(
        int idAsesor,
        string nombreAsesor,
        int? idSupervisorPeriodo = null,
        string? nombreSupervisorPeriodo = null,
        int? idSupervisorActual = null,
        string? nombreSupervisorActual = null)
    {
        return new RendimientoAsesorCarteraDbFila
        {
            IdAsesor = idAsesor,
            NombreAsesor = nombreAsesor,
            IdSupervisorPeriodo = idSupervisorPeriodo,
            NombreSupervisorPeriodo = nombreSupervisorPeriodo,
            IdSupervisorActual = idSupervisorActual,
            NombreSupervisorActual = nombreSupervisorActual,
            FechaDesde = new DateTime(2026, 9, 1),
            FechaHasta = new DateTime(2026, 9, 8),
            CantidadGestiones = 10,
            CantidadDeudoresGestionados = 7,
            MontoRecuperadoAtribuible = 0m
        };
    }
}
