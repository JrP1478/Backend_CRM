namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record CarteraOperacionResult<T>(
    T? Valor,
    int? CodigoEstadoError,
    string? TituloError,
    string? DetalleError,
    IReadOnlyDictionary<string, string[]>? ErroresValidacion)
{
    public bool EsExitoso => CodigoEstadoError is null && ErroresValidacion is null;

    public static CarteraOperacionResult<T> Exito(T valor) =>
        new(valor, null, null, null, null);

    public static CarteraOperacionResult<T> Problema(
        int codigoEstado,
        string? titulo,
        string? detalle) =>
        new(default, codigoEstado, titulo, detalle, null);

    public static CarteraOperacionResult<T> Validacion(
        IReadOnlyDictionary<string, string[]> errores) =>
        new(default, 400, "Solicitud inválida", "Uno o más parámetros no son válidos.", errores);

    public static CarteraOperacionResult<T> DesdeAcceso(
        CentroControlCarteraClienteAcceso acceso) =>
        Problema(
            acceso.CodigoEstadoError ?? 403,
            acceso.TituloError ?? "Acceso Analítica denegado",
            acceso.DetalleError ?? "No se pudo validar el acceso a Centro de Control de Cartera.");
}
