using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record EvolucionCarteraContexto(
    int ClaveCliente,
    int ClaveCampana,
    string CodigoCampana,
    string NombreCampana,
    DateOnly FechaInicio,
    DateOnly FechaFin,
    DateOnly? FechaUltimaEvolucion);
public readonly record struct RangoEvolucionCartera(
    DateOnly FechaDesde,
    DateOnly FechaHasta);
public sealed record EvolucionCarteraDbFila
{
    public DateTime Periodo { get; init; }
    public long CarteraAsignada { get; init; }
    public long CarteraGestionada { get; init; }
    public long CarteraPendiente { get; init; }
    public decimal MontoRecuperado { get; init; }
    public DateTime? FechaCargaUtc { get; init; }
}
