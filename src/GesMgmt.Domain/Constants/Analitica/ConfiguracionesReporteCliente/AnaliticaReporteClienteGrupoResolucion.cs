namespace GesMgmt.Domain.Constants.Analitica;

public static class AnaliticaReporteClienteGrupoResolucion
{
    public const string Configurado = "CONFIGURED";
    public const string DetectadoAutomaticamente = "AUTO_DETECTED";
    public const string Ambiguo = "AMBIGUOUS";
    public const string Faltante = "MISSING";
    public const string ConfiguracionInvalida = "INVALID_CONFIGURED";
    public const string NoDisponible = "UNAVAILABLE";

    public static bool EstaResuelto(string value) =>
        string.Equals(value, Configurado, StringComparison.Ordinal) ||
        string.Equals(value, DetectadoAutomaticamente, StringComparison.Ordinal);
}
