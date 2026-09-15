using GesMgmt.Application.Services.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.UnitTests.Analitica.Portfolio;

public sealed class RendimientoSupervisorCarteraResponseMapperTests
{
    [Fact]
    public void Map_ExposesManagedDebtorCount()
    {
        RendimientoSupervisorCarteraDbFila[] rows =
        [
            new()
            {
                IdSupervisor = 5,
                NombreSupervisor = "SUPERVISOR A",
                FechaDesde = new DateTime(2026, 9, 1),
                FechaHasta = new DateTime(2026, 9, 8),
                CantidadAsesores = 4,
                CantidadGestiones = 120,
                CantidadDeudoresGestionados = 73,
                MontoRecuperadoAtribuible = 0m
            }
        ];

        var response = RendimientoSupervisorCarteraResponseMapper.Map(rows);

        var supervisor = Assert.Single(response.Supervisores);
        Assert.Equal(73, supervisor.CantidadDeudoresGestionados);
    }
}
