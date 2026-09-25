namespace GesMgmt.Application.Interfaces.Analitica;

public interface IConsultaIncrustacionReporteClienteAnaliticaService
{
    Task<AnaliticaIncrustacionReporteClienteConsultaResult> ResolverAsync(
        int idOpcion,
        int idCliente,
        string reportClient,
        CancellationToken cancellationToken);
}

public enum AnaliticaIncrustacionReporteClienteConsultaEstado
{
    Exito,
    NoEncontrado,
    ConfiguracionInvalida
}

public sealed record AnaliticaIncrustacionReporteClienteConsultaResult(
    AnaliticaIncrustacionReporteClienteConsultaEstado Estado,
    string? UrlIncrustacion = null);
