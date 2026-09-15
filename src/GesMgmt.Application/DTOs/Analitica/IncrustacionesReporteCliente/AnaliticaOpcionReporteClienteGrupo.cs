using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaOpcionReporteClienteGrupo(
    int IdGrupo,
    string Nombre);
