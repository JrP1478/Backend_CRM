using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface IAccesoUsuarioPowerBiAnaliticaService
{
    Task<IReadOnlyList<AnaliticaPowerBiAccesoOpcion>> ResolverAsync(
        int idUsuario,
        IReadOnlyCollection<int> idsOpciones,
        CancellationToken cancellationToken);
}

public sealed record AnaliticaPowerBiAccesoOpcion(
    int IdOpcion,
    bool Permitido,
    bool RequiereSeleccionCliente);
