using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public static class AnaliticaPowerBiClienteSeleccionEstado
{
    public const string NoRequerida = "NOT_REQUIRED";
    public const string Valida = "VALID";
    public const string Faltante = "MISSING";
    public const string Invalido = "INVALID";
}

public sealed record AnaliticaVisorPowerBiSeleccion(
    int IdCliente,
    string Nombre);

public sealed record AnaliticaContextoVisorPowerBi(
    bool Permitido,
    bool RequiereSeleccionCliente,
    string EstadoSeleccionCliente,
    AnaliticaOpcionReporteCliente? ClienteSeleccionado,
    string? UrlIncrustacion);
