namespace GesMgmt.Domain.Entities.Analitica;

public sealed record SisgesGrupoCliente
{
    public int IdGrupo { get; init; }
    public int IdCliente { get; init; }
    public required string NombreGrupo { get; init; }
}
