using System.Diagnostics;

namespace GesMgmt.Application.Utils.Analitica.CentroControlCartera;

public static class DiagnosticosCentroControlCartera
{
    public const string FaseAcceso = "access";
    public const string FaseContexto = "context";
    public const string FaseConsulta = "query";

    private static readonly AsyncLocal<EstadoSolicitud?> SolicitudActual = new();
    private static readonly AsyncLocal<string?> FaseActual = new();

    public static IDisposable IniciarSolicitud(string endpoint)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(endpoint);

        var solicitudAnterior = SolicitudActual.Value;
        var faseAnterior = FaseActual.Value;
        SolicitudActual.Value = new EstadoSolicitud(endpoint);
        FaseActual.Value = null;

        return new AlcanceDelegado(() =>
        {
            SolicitudActual.Value = solicitudAnterior;
            FaseActual.Value = faseAnterior;
        });
    }

    public static async Task<T> ObservarFaseAsync<T>(
        string fase,
        Func<Task<T>> accion)
    {
        ArgumentNullException.ThrowIfNull(accion);

        using var phaseScope = IniciarFase(fase);
        return await accion();
    }

    public static IDisposable IniciarFase(string fase)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fase);

        var solicitud = SolicitudActual.Value;
        if (solicitud is null)
        {
            return AlcanceDelegado.Vacia;
        }

        var faseAnterior = FaseActual.Value;
        FaseActual.Value = fase;
        var iniciadoEn = Stopwatch.GetTimestamp();

        return new AlcanceDelegado(() =>
        {
            solicitud.RegistrarFase(
                fase,
                Stopwatch.GetElapsedTime(iniciadoEn));
            FaseActual.Value = faseAnterior;
        });
    }

    public static void RegistrarOperacionBaseDatos(
        string rolBaseDatos,
        string operacion,
        TimeSpan transcurrido)
    {
        var solicitud = SolicitudActual.Value;
        if (solicitud is null)
        {
            return;
        }

        solicitud.RegistrarOperacionBaseDatos(
            rolBaseDatos,
            operacion,
            FaseActual.Value,
            transcurrido);
    }

    public static CapturaRequestCentroControlCartera Capturar()
    {
        var solicitud = SolicitudActual.Value;
        return solicitud?.CrearInstantanea()
            ?? CapturaRequestCentroControlCartera.Vacia;
    }

    public static ContextoOperacionCentroControlCartera? ContextoOperacionActual()
    {
        var solicitud = SolicitudActual.Value;
        if (solicitud is null)
        {
            return null;
        }

        var fase = FaseActual.Value;
        return new ContextoOperacionCentroControlCartera(
            solicitud.Endpoint,
            string.IsNullOrWhiteSpace(fase)
                ? "unclassified"
                : fase!);
    }

    private sealed class EstadoSolicitud(string endpoint)
    {
        public string Endpoint => endpoint;

        private readonly object _bloqueo = new();
        private readonly Dictionary<string, double> _milisegundosPorFase =
            new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _comandosBaseDatosPorRol =
            new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _comandosBaseDatosPorFase =
            new(StringComparer.Ordinal);
        private double _milisegundosComandosBaseDatos;
        private int _cantidadComandosBaseDatos;
        private int _cantidadAperturasConexion;

        public void RegistrarFase(string fase, TimeSpan transcurrido)
        {
            lock (_bloqueo)
            {
                _milisegundosPorFase.TryGetValue(fase, out var actual);
                _milisegundosPorFase[fase] = actual + transcurrido.TotalMilliseconds;
            }
        }

        public void RegistrarOperacionBaseDatos(
            string rolBaseDatos,
            string operacion,
            string? fase,
            TimeSpan transcurrido)
        {
            lock (_bloqueo)
            {
                if (string.Equals(
                        operacion,
                        "connection.open",
                        StringComparison.Ordinal))
                {
                    _cantidadAperturasConexion++;
                    return;
                }

                _cantidadComandosBaseDatos++;
                _milisegundosComandosBaseDatos += transcurrido.TotalMilliseconds;

                _comandosBaseDatosPorRol.TryGetValue(rolBaseDatos, out var cantidadRol);
                _comandosBaseDatosPorRol[rolBaseDatos] = cantidadRol + 1;

                var faseNormalizada = string.IsNullOrWhiteSpace(fase)
                    ? "unclassified"
                    : fase;
                _comandosBaseDatosPorFase.TryGetValue(faseNormalizada, out var cantidadFase);
                _comandosBaseDatosPorFase[faseNormalizada] = cantidadFase + 1;
            }
        }

        public CapturaRequestCentroControlCartera CrearInstantanea()
        {
            lock (_bloqueo)
            {
                return new CapturaRequestCentroControlCartera(
                    endpoint,
                    ObtenerMilisegundosFase(FaseAcceso),
                    ObtenerMilisegundosFase(FaseContexto),
                    ObtenerMilisegundosFase(FaseConsulta),
                    _cantidadComandosBaseDatos,
                    _cantidadAperturasConexion,
                    _milisegundosComandosBaseDatos,
                    ObtenerCantidad(_comandosBaseDatosPorRol, "analytics"),
                    ObtenerCantidad(_comandosBaseDatosPorRol, "sisges"),
                    ObtenerCantidad(_comandosBaseDatosPorFase, FaseAcceso),
                    ObtenerCantidad(_comandosBaseDatosPorFase, FaseContexto),
                    ObtenerCantidad(_comandosBaseDatosPorFase, FaseConsulta),
                    ObtenerCantidad(_comandosBaseDatosPorFase, "unclassified"));
            }
        }

        private double ObtenerMilisegundosFase(string fase) =>
            _milisegundosPorFase.TryGetValue(fase, out var valor)
                ? valor
                : 0d;

        private static int ObtenerCantidad(
            IReadOnlyDictionary<string, int> origen,
            string clave) =>
            origen.TryGetValue(clave, out var valor)
                ? valor
                : 0;
    }

    private sealed class AlcanceDelegado(Action alLiberar) : IDisposable
    {
        public static readonly AlcanceDelegado Vacia = new(static () => { });

        private Action? _alLiberar = alLiberar;

        public void Dispose()
        {
            Interlocked.Exchange(ref _alLiberar, null)?.Invoke();
        }
    }
}

public sealed record CapturaRequestCentroControlCartera(
    string Endpoint,
    double MilisegundosAcceso,
    double MilisegundosContexto,
    double MilisegundosConsulta,
    int CantidadComandosBaseDatos,
    int CantidadAperturasConexion,
    double MilisegundosComandosBaseDatos,
    int ComandosBaseDatosAnalitica,
    int ComandosBaseDatosSisges,
    int ComandosBaseDatosAcceso,
    int ComandosBaseDatosContexto,
    int ComandosBaseDatosConsulta,
    int ComandosBaseDatosSinClasificar)
{
    public static readonly CapturaRequestCentroControlCartera Vacia = new(
        "unknown",
        0d,
        0d,
        0d,
        0,
        0,
        0d,
        0,
        0,
        0,
        0,
        0,
        0);
}

public sealed record ContextoOperacionCentroControlCartera(
    string Endpoint,
    string Fase);
