using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record RendimientoCampanaCarteraItem(
    string CodigoCampana,
    string NombreCampana,
    DateOnly FechaDesde,
    DateOnly FechaHasta,
    DateOnly FechaCorte,
    long CarteraAsignada,
    long CarteraGestionada,
    long CarteraPendiente,
    decimal? TasaAvance,
    long CantidadGestiones,
    decimal? TasaContactabilidad,
    decimal? TasaContactoDirecto,
    decimal? TasaCierre,
    long CantidadPromesas,
    decimal? TasaCumplimientoPromesa,
    long CantidadPagos,
    decimal MontoRecuperado,
    decimal? MontoMeta);
public sealed record RendimientoCampanaCarteraResponse(
    DateTimeOffset? FechaActualizacion,
    IReadOnlyList<RendimientoCampanaCarteraItem> Campanas);
