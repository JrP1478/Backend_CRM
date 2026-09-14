namespace GesMgmt.Domain.Entities.Analitica;

public sealed record AnaliticaAlcanceClienteAutorizadoEntrada
{
    public int IdClienteCrm { get; init; }
    public required string Nombre { get; init; }
}
