using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAccesoReporteClienteAnaliticaService
{
    Task<AnaliticaReporteClienteAccesoResult> ResolverAsync(
        int idUsuario,
        int idOpcion,
        CancellationToken cancellationToken);
}

public sealed record AnaliticaReporteClienteAccesoResult(
    bool TieneAccesoOpcion,
    IReadOnlyList<AnaliticaOpcionReporteCliente> Clientes);
