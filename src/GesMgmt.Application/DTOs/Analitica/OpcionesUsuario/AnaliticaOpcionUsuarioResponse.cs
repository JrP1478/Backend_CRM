using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaOpcionUsuarioResponse(
    int IdOpcion,
    string CodigoOpcion,
    string NombreOpcion);
