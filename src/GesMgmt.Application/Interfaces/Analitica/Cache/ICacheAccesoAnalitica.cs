using GesMgmt.Application.DTOs.Analitica;
using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.Interfaces.Analitica;

public interface ICacheAccesoAnalitica
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        TimeSpan duration,
        Func<CancellationToken, Task<T>> factory,
        CancellationToken cancellationToken);

    void Remove(string key);
}
