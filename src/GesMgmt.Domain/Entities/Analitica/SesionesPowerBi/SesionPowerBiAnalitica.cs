namespace GesMgmt.Domain.Entities.Analitica.SesionesPowerBi;

public sealed class SesionPowerBiAnalitica
{
    public Guid IdSesion { get; set; }
    public int IdUsuario { get; set; }
    public int IdOpcionReporte { get; set; }
    public int? IdCliente { get; set; }
    public string UsuarioLogin { get; set; } = string.Empty;
    public string UsuarioNombre { get; set; } = string.Empty;
    public string ReporteNombre { get; set; } = string.Empty;
    public string? ClienteNombre { get; set; }
    public DateTime FechaInicioUtc { get; set; }
    public DateTime FechaUltimoHeartbeatUtc { get; set; }
    public DateTime? FechaFinUtc { get; set; }
    public int SegundosVisibles { get; set; }
    public bool EstaVisible { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? MotivoCierre { get; set; }
    public DateTime FechaCreacionUtc { get; set; }
    public DateTime FechaActualizacionUtc { get; set; }
}
