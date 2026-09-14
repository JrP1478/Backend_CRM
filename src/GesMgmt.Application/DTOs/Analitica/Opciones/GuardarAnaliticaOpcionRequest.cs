using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record GuardarAnaliticaOpcionRequest(
    string CodigoOpcion,
    string NombreOpcion,
    bool EsActivo);
