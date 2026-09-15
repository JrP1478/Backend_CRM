using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record PromesasCarteraCampana(
    string Code,
    string Nombre);
public sealed record PromesaCarteraEstadoMetricas(
    long CantidadVenceHoy,
    decimal MontoVenceHoy,
    long CantidadVencidas,
    decimal? TasaCumplimiento);
public sealed record PromesasCarteraResponse(
    PromesasCarteraCampana Campana,
    DateTimeOffset? FechaActualizacion,
    PromesaCarteraEstadoMetricas Promises);
