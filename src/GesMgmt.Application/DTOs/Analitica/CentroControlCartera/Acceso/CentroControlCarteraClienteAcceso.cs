namespace GesMgmt.Application.DTOs.Analitica.CentroControlCartera;

public sealed record CentroControlCarteraClienteAcceso(
    int? IdClienteCrm,
    int? CodigoEstadoError,
    string? TituloError,
    string? DetalleError)
{
    public bool EstaPermitido => IdClienteCrm.HasValue;

    public static CentroControlCarteraClienteAcceso Permitido(int idClienteCrm) =>
        new(idClienteCrm, null, null, null);

    public static CentroControlCarteraClienteAcceso Error(
        int codigoEstado,
        string titulo,
        string detalle) =>
        new(null, codigoEstado, titulo, detalle);
}
