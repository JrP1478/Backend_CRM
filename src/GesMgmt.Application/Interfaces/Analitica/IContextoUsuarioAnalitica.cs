namespace GesMgmt.Application.Interfaces.Analitica
{
    /// <resumen>
    /// Expone la identidad CRM y el grupo funcional actual a los casos de uso
    /// de Analítica sin acoplar Application al mecanismo HTTP/autenticación.
    /// </resumen>
    public interface IContextoUsuarioAnalitica
    {
        bool IntentarObtenerIdUsuario(out int idUsuario);
        bool IntentarObtenerIdGrupo(out int idGrupo);
    }
}
