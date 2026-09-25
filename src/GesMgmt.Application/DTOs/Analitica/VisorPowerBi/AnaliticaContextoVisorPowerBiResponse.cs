using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaContextoVisorPowerBiResponse(
    int IdOpcion,
    bool Permitido,
    bool RequiereSeleccionCliente,
    string EstadoSeleccionCliente,
    AnaliticaOpcionReporteCliente? ClienteSeleccionado,
    string? UrlIncrustacion);
