using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record RendimientoSupervisorCarteraDbFila
{
    public int IdSupervisor { get; init; }
    public required string NombreSupervisor { get; init; }
    public DateTime FechaDesde { get; init; }
    public DateTime FechaHasta { get; init; }
    public long CantidadAsesores { get; init; }
    public long CantidadGestiones { get; init; }
    public long CantidadDeudoresGestionados { get; init; }
    public decimal? TasaContactoDirecto { get; init; }
    public decimal? TasaCierre { get; init; }
    public long CantidadPromesas { get; init; }
    public decimal? TasaCumplimientoPromesa { get; init; }
    public long CantidadPagos { get; init; }
    public decimal MontoRecuperadoAtribuible { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
