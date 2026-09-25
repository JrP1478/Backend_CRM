namespace GesMgmt.Domain.Entities.Analitica;

public sealed class AnaliticaCatalogoReporteClienteEntrada
{
    public int IdOpcion { get; set; }
    public int IdClienteCrm { get; set; }
    public string ClienteReporte { get; set; } = string.Empty;
}
