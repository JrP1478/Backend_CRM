using GesMgmt.Application.DTOs.Analitica.CentroControlCartera;
using GesMgmt.Domain.Entities.Analitica.CentroControlCartera;
using GesMgmt.Domain.Interfaces.Analitica.CentroControlCartera;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories.Analitica.CentroControlCartera;

internal sealed class ResumenCarteraRepository(AnaliticaDbContext context)
    : IResumenCarteraRepository
{
    public async Task<ResumenCarteraContexto?> ResolverContextoAsync(
        int idClienteCrm,
        string? codigoCampana,
        long? idSubCartera,
        string? unidadNegocio,
        CancellationToken cancellationToken)
    {
        var overviewContext = await PanoramaCarteraContextoEfConsulta.EjecutarAsync(
            context,
            idClienteCrm,
            codigoCampana,
            idSubCartera,
            unidadNegocio,
            cancellationToken);

        return overviewContext?.Resumen;
    }

    public Task<ResumenCarteraDbFila> ObtenerResumenAsync(
        int claveCliente,
        int claveCampana,
        long? idSubCartera,
        string? unidadNegocio,
        RangoResumenCartera range,
        CancellationToken cancellationToken) =>
        ResumenCarteraEfConsulta.EjecutarAsync(
            context,
            claveCliente,
            claveCampana,
            idSubCartera,
            unidadNegocio,
            range,
            cancellationToken);
}
