using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record RendimientoCampanaCarteraDbFila
{
    public required string CodigoCampana { get; init; }
    public required string NombreCampana { get; init; }
    public DateTime FechaDesde { get; init; }
    public DateTime FechaHasta { get; init; }
    public DateTime FechaCorte { get; init; }
    public long CarteraAsignada { get; init; }
    public long CarteraGestionada { get; init; }
    public long CarteraPendiente { get; init; }
    public decimal? TasaAvance { get; init; }
    public long CantidadGestiones { get; init; }
    public decimal? TasaContactabilidad { get; init; }
    public decimal? TasaContactoDirecto { get; init; }
    public decimal? TasaCierre { get; init; }
    public long CantidadPromesas { get; init; }
    public decimal? TasaCumplimientoPromesa { get; init; }
    public long CantidadPagos { get; init; }
    public decimal MontoRecuperado { get; init; }
    public decimal? MontoMeta { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
