using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

public static class SisgesGrupoClienteSql
{
    public const string ObtenerTodosGruposActivos = """
        SELECT DISTINCT
            g.nId_Grupo AS IdGrupo,
            g.nid_cliente AS IdCliente,
            LTRIM(RTRIM(COALESCE(g.cNombre_Grupo, ''))) AS NombreGrupo
        FROM dbo.av_Grupo AS g
        WHERE g.bEstado = 1
          AND g.nid_cliente IS NOT NULL
          AND g.nid_cliente > 0
        ORDER BY
            NombreGrupo,
            g.nId_Grupo;
        """;

    public const string ObtenerGruposActivos = """
        SELECT DISTINCT
            g.nId_Grupo AS IdGrupo,
            g.nid_cliente AS IdCliente,
            LTRIM(RTRIM(COALESCE(g.cNombre_Grupo, ''))) AS NombreGrupo
        FROM dbo.av_Grupo AS g
        WHERE g.bEstado = 1
          AND g.nid_cliente IN @IdsClientes
          AND g.nid_cliente IS NOT NULL
          AND g.nid_cliente > 0
        ORDER BY
            g.nid_cliente,
            g.nId_Grupo;
        """;
}
