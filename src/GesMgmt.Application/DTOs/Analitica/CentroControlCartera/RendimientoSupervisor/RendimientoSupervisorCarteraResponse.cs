using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record RendimientoSupervisorCarteraItem(
    int IdSupervisor,
    string NombreSupervisor,
    long CantidadAsesores,
    long CantidadGestiones,
    long CantidadDeudoresGestionados,
    decimal? TasaContactoDirecto,
    decimal? TasaCierre,
    long CantidadPromesas,
    decimal? TasaCumplimientoPromesa,
    long CantidadPagos,
    decimal MontoRecuperadoAtribuible);
public sealed record RendimientoSupervisorCarteraResponse(
    DateOnly? FechaDesde,
    DateOnly? FechaHasta,
    DateTimeOffset? FechaActualizacion,
    IReadOnlyList<RendimientoSupervisorCarteraItem> Supervisores);
