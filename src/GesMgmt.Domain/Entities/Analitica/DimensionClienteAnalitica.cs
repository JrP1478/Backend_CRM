namespace GesMgmt.Domain.Entities.Analitica;

public sealed class DimensionClienteAnalitica
{
    public int ClaveCliente { get; set; }
    public int IdClienteCrm { get; set; }
    public string? CodigoCliente { get; set; }
    public string? NombreCliente { get; set; }
}
