namespace GesMgmt.Domain.Entities.Analitica;

public sealed record CrmGrupoCliente
{
    public int IdGrupo { get; init; }
    public int IdCliente { get; init; }
    public required string NombreGrupo { get; init; }
}
