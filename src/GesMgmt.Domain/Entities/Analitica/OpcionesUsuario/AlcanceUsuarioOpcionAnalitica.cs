namespace GesMgmt.Domain.Entities.Analitica;

public sealed class AlcanceUsuarioOpcionAnalitica
{
    public int IdUsuario { get; set; }
    public int IdOpcion { get; set; }
    public bool EsActivo { get; set; }
    public int? CreadoPor { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int? ActualizadoPor { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
