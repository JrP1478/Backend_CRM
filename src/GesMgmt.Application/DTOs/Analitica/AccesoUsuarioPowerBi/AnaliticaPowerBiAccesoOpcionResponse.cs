using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaPowerBiAccesoOpcionResponse(
    int IdOpcion,
    bool Permitido,
    bool RequiereSeleccionCliente);
