using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record EvolucionCarteraCampana(
    string Code,
    string Nombre);
public sealed record EvolucionCarteraPeriodo(
    DateOnly FechaDesde,
    DateOnly FechaHasta);
public sealed record EvolucionCarteraPunto(
    DateOnly Periodo,
    long CarteraAsignada,
    long CarteraGestionada,
    long CarteraPendiente,
    decimal MontoRecuperado);
public sealed record EvolucionCarteraResponse(
    EvolucionCarteraCampana Campana,
    EvolucionCarteraPeriodo Periodo,
    DateTimeOffset? FechaActualizacion,
    IReadOnlyList<EvolucionCarteraPunto> Evolucion);
