namespace GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;

public sealed class EventoSesionPowerBiAnalitica
{
    public long IdEvento { get; set; }
    public Guid IdSesion { get; set; }
    public string TipoEvento { get; set; } = string.Empty;
    public DateTime FechaEventoUtc { get; set; }
    public int SegundosVisibles { get; set; }
    public string Origen { get; set; } = string.Empty;
    public string? Detalle { get; set; }
}
