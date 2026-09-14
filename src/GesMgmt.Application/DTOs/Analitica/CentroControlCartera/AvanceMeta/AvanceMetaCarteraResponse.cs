using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record AvanceMetaCarteraCampana(
    string Code,
    string Nombre);
public sealed record AvanceMetaCarteraPeriodo(
    DateOnly FechaHasta,
    DateOnly? FechaCorte);
public sealed record AvanceMetaCarteraMetricas(
    decimal MontoMetaMensual,
    decimal MontoEsperadoFecha,
    decimal? TasaCumplimientoMeta,
    decimal? TasaCumplimientoRitmo,
    decimal MontoBrecha,
    decimal? TasaBrecha);
public sealed record AvanceMetaCarteraResponse(
    AvanceMetaCarteraCampana Campana,
    AvanceMetaCarteraPeriodo Periodo,
    DateTimeOffset? FechaActualizacion,
    AvanceMetaCarteraMetricas? Meta);
