using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record PromesaVenceHoyCarteraDbFila
{
    public long IdPromesa { get; init; }
    public long IdDeudor { get; init; }
    public DateTime? FechaVencimiento { get; init; }
    public decimal MontoPromesa { get; init; }
    public decimal MontoPagado { get; init; }
    public decimal MontoPendiente { get; init; }
    public DateTime? FechaUltimoPago { get; init; }
    public required string ClaveEstado { get; init; }
    public int? IdAsesor { get; init; }
    public string? NombreAsesor { get; init; }
    public int? IdSupervisor { get; init; }
    public string? NombreSupervisor { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
public sealed record PromesasVencenHoyCarteraMetadatosDbFila
{
    public string? ClaveEstado { get; init; }
    public long CantidadPromesas { get; init; }
    public decimal MontoPromesa { get; init; }
    public decimal MontoPagado { get; init; }
    public decimal MontoPendiente { get; init; }
    public DateTime? FechaCorte { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
public sealed record PromesasVencenHoyCarteraResumenDbFila
{
    public long CantidadVenceHoy { get; init; }
    public decimal MontoVenceHoy { get; init; }
    public decimal MontoPagado { get; init; }
    public decimal MontoPendiente { get; init; }
    public DateTime? FechaCorte { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
public sealed record PromesasVencenHoyCarteraEstadoDbFila
{
    public required string ClaveEstado { get; init; }
    public long CantidadPromesas { get; init; }
    public decimal MontoPromesa { get; init; }
    public decimal MontoPagado { get; init; }
    public decimal MontoPendiente { get; init; }
}
public sealed record PromesasVencenHoyCarteraConsultaResult(
    PromesasVencenHoyCarteraResumenDbFila Resumen,
    IReadOnlyList<PromesasVencenHoyCarteraEstadoDbFila> Estado,
    IReadOnlyList<PromesaVenceHoyCarteraDbFila> Elementos,
    PromesasCarteraPaginacion Paginacion);
