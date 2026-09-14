using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaOpcionConfiguracionResponse(
    int IdOpcion,
    string CodigoOpcion,
    string NombreOpcion);
