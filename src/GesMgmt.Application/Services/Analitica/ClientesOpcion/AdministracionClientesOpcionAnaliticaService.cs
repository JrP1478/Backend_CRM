using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Application.Validators.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Application.Services.Analitica;

public sealed class AdministracionClientesOpcionAnaliticaService(
    IAnaliticaOpcionConfiguracionRepository optionRepository,
    IAnaliticaAlcanceClienteOpcionRepository repository)
    : IAdministracionClientesOpcionAnaliticaService
{
    public async Task<AnaliticaClientesOpcionResponse?> ObtenerAsync(
        int idOpcion,
        CancellationToken cancellationToken)
    {
        if (!await optionRepository.ExisteAsync(idOpcion, cancellationToken))
        {
            return null;
        }

        var idsClientes = await repository.ObtenerIdsClientesAsync(
            idOpcion,
            cancellationToken);

        return new AnaliticaClientesOpcionResponse(idOpcion, idsClientes);
    }

    public async Task<AnaliticaAdministracionComandoResult> ActualizarAsync(
        int idOpcion,
        IReadOnlyCollection<int>? requestedClientIds,
        int idUsuario,
        CancellationToken cancellationToken)
    {
        if (!await optionRepository.ExisteAsync(idOpcion, cancellationToken))
        {
            return AnaliticaAdministracionComandoResult.NoEncontrado();
        }

        var idsClientes = requestedClientIds ?? [];

        if (idsClientes.Any(idCliente => idCliente <= 0))
        {
            return AnaliticaAdministracionComandoResult.Invalido(
                "Clientes inválidos",
                "Todos los idsClientes deben ser enteros positivos.");
        }

        var newClientIds = idsClientes
            .Distinct()
            .OrderBy(idCliente => idCliente)
            .ToArray();
        var previousClientIds = await repository.ObtenerIdsClientesAsync(
            idOpcion,
            cancellationToken);
        var normalizedPreviousClientIds = previousClientIds
            .Distinct()
            .OrderBy(idCliente => idCliente)
            .ToArray();

        if (!normalizedPreviousClientIds.SequenceEqual(newClientIds))
        {
            await repository.ReemplazarAsync(
                idOpcion,
                normalizedPreviousClientIds,
                newClientIds,
                idUsuario,
                cancellationToken);
        }

        return AnaliticaAdministracionComandoResult.Exito();
    }
}
