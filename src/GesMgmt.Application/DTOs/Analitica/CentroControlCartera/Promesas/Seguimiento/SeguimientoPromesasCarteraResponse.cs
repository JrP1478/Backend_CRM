using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record SeguimientoPromesasCarteraResumen(
    long CantidadPromesas,
    decimal MontoPromesa,
    decimal MontoPagado,
    decimal MontoPendiente);

public sealed record SeguimientoPromesasCarteraEstadoRango(
    string Clave,
    string Etiqueta,
    long Cantidad,
    decimal MontoPromesa,
    decimal MontoPagado,
    decimal MontoPendiente);

public sealed record SeguimientoPromesaCarteraItem(
    long IdPromesa,
    long IdDeudor,
    string? NombreDeudor,
    DateOnly? FechaVencimiento,
    decimal MontoPromesa,
    decimal MontoPagado,
    decimal MontoPendiente,
    DateOnly? FechaUltimoPago,
    string ClaveEstado,
    bool Gestionado,
    long CantidadGestiones,
    long CantidadLlamadas,
    string ClaveContacto,
    string EtiquetaContacto,
    bool? ConfirmoPago,
    DateTime? FechaUltimaGestion,
    int? IdAsesor,
    string? NombreAsesor,
    int? IdSupervisor,
    string? NombreSupervisor);

public sealed record SeguimientoPromesasCarteraResponse(
    PromesasCarteraCampana Campana,
    DateOnly FechaVencimiento,
    DateOnly? FechaCorte,
    DateTimeOffset? FechaActualizacion,
    SeguimientoPromesasCarteraResumen Resumen,
    IReadOnlyList<SeguimientoPromesasCarteraEstadoRango> Estado,
    PromesasCarteraPaginacion Paginacion,
    IReadOnlyList<SeguimientoPromesaCarteraItem> Elementos);
