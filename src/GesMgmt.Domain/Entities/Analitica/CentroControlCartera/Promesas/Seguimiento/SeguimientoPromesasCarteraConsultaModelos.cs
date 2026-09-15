namespace GesMgmt.Domain.Entities.Analitica.CentroControlCartera;

public sealed record SeguimientoPromesaCarteraDbFila
{
    public long IdPromesa { get; init; }
    public long IdDeudor { get; init; }
    public string? NombreDeudor { get; init; }
    public DateTime? FechaVencimiento { get; init; }
    public decimal MontoPromesa { get; init; }
    public decimal MontoPagado { get; init; }
    public decimal MontoPendiente { get; init; }
    public DateTime? FechaUltimoPago { get; init; }
    public required string ClaveEstado { get; init; }
    public bool Gestionado { get; init; }
    public long CantidadGestiones { get; init; }
    public long CantidadLlamadas { get; init; }
    public required string ClaveContacto { get; init; }
    public bool? ConfirmoPago { get; init; }
    public DateTime? FechaUltimaGestion { get; init; }
    public int? IdAsesor { get; init; }
    public string? NombreAsesor { get; init; }
    public int? IdSupervisor { get; init; }
    public string? NombreSupervisor { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}

public sealed record SeguimientoPromesasCarteraResumenDbFila
{
    public long CantidadPromesas { get; init; }
    public decimal MontoPromesa { get; init; }
    public decimal MontoPagado { get; init; }
    public decimal MontoPendiente { get; init; }
    public DateTime? FechaCorte { get; init; }
    public DateTime? FechaActualizacionUtc { get; init; }
}

public sealed record SeguimientoPromesasCarteraEstadoDbFila
{
    public required string ClaveEstado { get; init; }
    public long CantidadPromesas { get; init; }
    public decimal MontoPromesa { get; init; }
    public decimal MontoPagado { get; init; }
    public decimal MontoPendiente { get; init; }
}

public sealed record SeguimientoPromesasCarteraConsultaResult(
    DateOnly FechaVencimiento,
    SeguimientoPromesasCarteraResumenDbFila Resumen,
    IReadOnlyList<SeguimientoPromesasCarteraEstadoDbFila> Estado,
    IReadOnlyList<SeguimientoPromesaCarteraDbFila> Elementos,
    PromesasCarteraPaginacion Paginacion);
