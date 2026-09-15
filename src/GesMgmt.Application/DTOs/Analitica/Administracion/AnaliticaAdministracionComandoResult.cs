using GesMgmt.Domain.Entities.Analitica;

namespace GesMgmt.Application.DTOs.Analitica;

public enum AnaliticaAdministracionComandoEstado
{
    Exito,
    NoEncontrado,
    SolicitudInvalida
}

public sealed record AnaliticaAdministracionComandoResult(
    AnaliticaAdministracionComandoEstado Estado,
    string? Titulo = null,
    string? Detalle = null)
{
    public static AnaliticaAdministracionComandoResult Exito() =>
        new(AnaliticaAdministracionComandoEstado.Exito);

    public static AnaliticaAdministracionComandoResult NoEncontrado() =>
        new(AnaliticaAdministracionComandoEstado.NoEncontrado);

    public static AnaliticaAdministracionComandoResult Invalido(
        string title,
        string detail) =>
        new(
            AnaliticaAdministracionComandoEstado.SolicitudInvalida,
            title,
            detail);
}
