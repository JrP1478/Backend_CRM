using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record RendimientoAsesorCarteraItem(
    int IdAsesor,
    string NombreAsesor,
    int? IdSupervisorPeriodo,
    string? NombreSupervisorPeriodo,
    int? IdSupervisorActual,
    string? NombreSupervisorActual,
    long CantidadGestiones,
    long CantidadDeudoresGestionados,
    decimal? TasaContactoDirecto,
    decimal? TasaCierre,
    long CantidadPromesas,
    long CantidadPagos,
    decimal MontoRecuperadoAtribuible);
public sealed record RendimientoAsesorCarteraResponse(
    DateOnly? FechaDesde,
    DateOnly? FechaHasta,
    DateTimeOffset? FechaActualizacion,
    IReadOnlyList<RendimientoAsesorCarteraItem> Asesores);
