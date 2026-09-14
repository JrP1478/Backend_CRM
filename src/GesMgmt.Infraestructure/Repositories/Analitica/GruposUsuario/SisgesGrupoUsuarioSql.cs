using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

public static class SisgesGrupoUsuarioSql
{
    public const string ObtenerGruposActivos = """
        SELECT DISTINCT ug.nId_Grupo
        FROM dbo.av_UGrupo AS ug
        INNER JOIN dbo.av_Grupo AS g
            ON g.nId_Grupo = ug.nId_Grupo
        WHERE ug.nId_Usuario = @IdUsuario
          AND ug.bEstado = 1
          AND ug.bActivo = 1
          AND g.bEstado = 1
        ORDER BY ug.nId_Grupo;
        """;
}
