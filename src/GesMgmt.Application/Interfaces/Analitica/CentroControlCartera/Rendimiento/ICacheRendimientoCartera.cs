namespace GesMgmt.Application.Interfaces.Analitica.CentroControlCartera;

public interface ICacheRendimientoCartera
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        TimeSpan duration,
        Func<CancellationToken, Task<T>> factory,
        CancellationToken cancellationToken);
}
