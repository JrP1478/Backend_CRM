using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record PromesasCarteraContexto(
    int ClaveCliente,
    int ClaveCampana,
    string CodigoCampana,
    string NombreCampana);
public sealed record PromesasCarteraDbFila
{
    public long CantidadVenceHoy { get; init; }
    public decimal MontoVenceHoy { get; init; }
    public long CantidadVencidas { get; init; }
    public decimal? TasaCumplimiento { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
