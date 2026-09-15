namespace GesMgmt.Domain.Constants.Analitica.SesionesPowerBi;

public static class EstadoSesionPowerBi
{
    public const string Activa = "ACTIVA";
    public const string Pausada = "PAUSADA";
    public const string Cerrada = "CERRADA";
    public const string Expirada = "EXPIRADA";

    public static bool EsFinal(string estado) =>
        string.Equals(estado, Cerrada, StringComparison.Ordinal) ||
        string.Equals(estado, Expirada, StringComparison.Ordinal);
}
