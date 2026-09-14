using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record ResumenCarteraCampana(
    string Code,
    string Nombre);
public sealed record ResumenCarteraPeriodo(
    DateOnly FechaDesde,
    DateOnly FechaHasta,
    DateOnly FechaCorte);
public sealed record ResumenCarteraVigencia(
    DateTimeOffset? OperationAsOfAt,
    DateTimeOffset? PortfolioBaseRefreshedAt,
    DateTimeOffset? RefreshedAt);
public sealed record ResumenCarteraMetricas(
    long CarteraAsignada,
    long CarteraGestionada,
    long CarteraPendiente,
    long CantidadGestiones,
    decimal? IntensidadGestion,
    decimal MontoRecuperado,
    decimal? TasaContactabilidad,
    decimal? TasaContactoDirecto,
    decimal? TasaCierre,
    long CantidadPromesas,
    decimal? TasaCumplimientoPromesa,
    long CantidadPagos);
public sealed record ResumenCarteraResponse(
    ResumenCarteraCampana Campana,
    ResumenCarteraPeriodo Periodo,
    DateTimeOffset? FechaActualizacion,
    ResumenCarteraVigencia Vigencia,
    ResumenCarteraMetricas Resumen);
