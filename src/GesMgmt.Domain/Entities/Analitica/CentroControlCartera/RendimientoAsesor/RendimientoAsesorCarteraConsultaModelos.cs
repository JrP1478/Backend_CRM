using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record RendimientoAsesorCarteraDbFila
{
    public int IdAsesor { get; init; }
    public required string NombreAsesor { get; init; }
    public int? IdSupervisorPeriodo { get; init; }
    public string? NombreSupervisorPeriodo { get; init; }
    public int? IdSupervisorActual { get; init; }
    public string? NombreSupervisorActual { get; init; }
    public DateTime FechaDesde { get; init; }
    public DateTime FechaHasta { get; init; }
    public long CantidadGestiones { get; init; }
    public long CantidadDeudoresGestionados { get; init; }
    public decimal? TasaContactoDirecto { get; init; }
    public decimal? TasaCierre { get; init; }
    public long CantidadPromesas { get; init; }
    public long CantidadPagos { get; init; }
    public decimal MontoRecuperadoAtribuible { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
