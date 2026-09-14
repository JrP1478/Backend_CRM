using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record PromesasVencenHoyCarteraResumen(
    long CantidadVenceHoy,
    decimal MontoVenceHoy,
    decimal MontoPagado,
    decimal MontoPendiente);
public sealed record PromesasVencenHoyCarteraEstadoRango(
    string Clave,
    string Etiqueta,
    long Cantidad,
    decimal MontoPromesa,
    decimal MontoPagado,
    decimal MontoPendiente);
public sealed record PromesaVenceHoyCarteraItem(
    long IdPromesa,
    long IdDeudor,
    decimal MontoPromesa,
    decimal MontoPagado,
    decimal MontoPendiente,
    DateOnly? FechaUltimoPago,
    string ClaveEstado,
    int? IdAsesor,
    string? NombreAsesor,
    int? IdSupervisor,
    string? NombreSupervisor);
public sealed record PromesasVencenHoyCarteraResponse(
    PromesasCarteraCampana Campana,
    DateOnly? FechaCorte,
    DateTimeOffset? FechaActualizacion,
    PromesasVencenHoyCarteraResumen Resumen,
    IReadOnlyList<PromesasVencenHoyCarteraEstadoRango> Estado,
    PromesasCarteraPaginacion Paginacion,
    IReadOnlyList<PromesaVenceHoyCarteraItem> Elementos);
