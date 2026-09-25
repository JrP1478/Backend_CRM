using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record ResumenCarteraContexto(
    int ClaveCliente,
    int ClaveCampana,
    string CodigoCampana,
    string NombreCampana,
    DateOnly FechaInicio,
    DateOnly FechaFin,
    DateOnly? FechaUltimoDato);
public readonly record struct RangoResumenCartera(
    DateOnly FechaDesde,
    DateOnly FechaHasta);
public sealed record ResumenCarteraDbFila
{
    public DateTime? FechaCorte { get; init; }
    public long CarteraAsignada { get; init; }
    public long CarteraGestionada { get; init; }
    public long CarteraPendiente { get; init; }
    public long CantidadGestiones { get; init; }
    public decimal? IntensidadGestion { get; init; }
    public decimal MontoRecuperado { get; init; }
    public decimal? TasaContactabilidad { get; init; }
    public decimal? TasaContactoDirecto { get; init; }
    public decimal? TasaCierre { get; init; }
    public long CantidadPromesas { get; init; }
    public decimal? TasaCumplimientoPromesa { get; init; }
    public long CantidadPagos { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
    public DateTime? FechaCorteOperacionLocal { get; init; }
    public DateTime? FechaActualizacionBaseCarteraUtc { get; init; }
    public DateTime? FechaActualizacionDatosUtc { get; init; }
}
