using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record AvanceMetaCarteraContexto(
    int ClaveCliente,
    int ClaveCampana,
    string CodigoCampana,
    string NombreCampana,
    DateOnly FechaInicio,
    DateOnly FechaFin,
    DateOnly? LatestProgressDate);
public sealed record AvanceMetaCarteraDbFila
{
    public DateTime FechaCorte { get; init; }
    public decimal? MontoMetaMensual { get; init; }
    public decimal? MontoEsperadoFecha { get; init; }
    public decimal? TasaCumplimientoMeta { get; init; }
    public decimal? TasaCumplimientoRitmo { get; init; }
    public decimal? MontoBrecha { get; init; }
    public decimal? TasaBrecha { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}
