using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

public static class CrmGrupoUsuarioSql
{
    public const string ObtenerGruposActivos = """
        SELECT DISTINCT ug.nId_Grupo
        FROM dbo.Crm_UGrupo AS ug
        INNER JOIN dbo.Crm_Grupo AS g
            ON g.nId_Grupo = ug.nId_Grupo
        WHERE ug.nId_Usuario = @IdUsuario
          AND ug.bEstado = 1
          AND ug.bActivo = 1
          AND g.bEstado = 1
        ORDER BY ug.nId_Grupo;
        """;
}
