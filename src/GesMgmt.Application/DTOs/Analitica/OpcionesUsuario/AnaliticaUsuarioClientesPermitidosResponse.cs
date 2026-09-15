using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaUsuarioClientePermitido(
    int IdCliente,
    string Nombre);

public sealed record AnaliticaUsuarioClientesPermitidosResponse(
    int IdOpcion,
    IReadOnlyList<int> IdsClientes,
    IReadOnlyList<AnaliticaUsuarioClientePermitido> Clientes)
{
    public static AnaliticaUsuarioClientesPermitidosResponse DesdeIds(
        int idOpcion,
        IReadOnlyList<int> idsClientes) =>
        new(
            idOpcion,
            idsClientes,
            Array.Empty<AnaliticaUsuarioClientePermitido>());

    public static AnaliticaUsuarioClientesPermitidosResponse DesdeClientes(
        int idOpcion,
        IReadOnlyList<AnaliticaClientePermitido> clientes) =>
        new(
            idOpcion,
            clientes.Select(client => client.IdClienteCrm).ToArray(),
            clientes.Select(client => new AnaliticaUsuarioClientePermitido(
                client.IdClienteCrm,
                client.Nombre)).ToArray());
}
