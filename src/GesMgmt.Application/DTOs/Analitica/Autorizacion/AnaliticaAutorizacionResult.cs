using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public sealed record AnaliticaAutorizacionResult(
    bool Permitido,
    string? Reason = null)
{
    public static AnaliticaAutorizacionResult Permitir() =>
        new(true);

    public static AnaliticaAutorizacionResult Denegar(
        string reason) =>
        new(false, reason);
}
