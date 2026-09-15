using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class ConsultaIncrustacionReporteClienteAnaliticaService(
    IAnaliticaIncrustacionReporteClienteRepository repository)
    : IConsultaIncrustacionReporteClienteAnaliticaService
{
    public async Task<AnaliticaIncrustacionReporteClienteConsultaResult> ResolverAsync(
        int idOpcion,
        int idCliente,
        string reportClient,
        CancellationToken cancellationToken)
    {
        var mapping = await repository.ObtenerAsync(
            idOpcion,
            idCliente,
            reportClient,
            cancellationToken);

        if (mapping is null)
        {
            return new AnaliticaIncrustacionReporteClienteConsultaResult(
                AnaliticaIncrustacionReporteClienteConsultaEstado.NoEncontrado);
        }

        if (!UrlPublicacionWebPowerBi.IntentarNormalizar(
            mapping.UrlIncrustacion,
            out var urlIncrustacion))
        {
            return new AnaliticaIncrustacionReporteClienteConsultaResult(
                AnaliticaIncrustacionReporteClienteConsultaEstado.ConfiguracionInvalida);
        }

        return new AnaliticaIncrustacionReporteClienteConsultaResult(
            AnaliticaIncrustacionReporteClienteConsultaEstado.Exito,
            urlIncrustacion);
    }
}
