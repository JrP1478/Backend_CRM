using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

public static class CrmClienteUsuarioSql
{
    public const string GetActiveClients = """
SELECT DISTINCT g.nid_cliente
FROM dbo.Crm_Usuario u
INNER JOIN dbo.Crm_UGrupo ug ON ug.nId_Usuario = u.nId_Usuario
INNER JOIN dbo.Crm_Grupo g ON g.nId_Grupo = ug.nId_Grupo
WHERE u.nId_Usuario = @IdUsuario
AND u.bEstado = 1
AND ug.bEstado = 1
AND ug.bActivo = 1
AND g.bEstado = 1
AND g.nid_cliente IS NOT NULL
AND g.nid_cliente > 0
ORDER BY g.nid_cliente;
""";

    public const string IsActiveClient = """
SELECT TOP (1) 1
FROM dbo.Crm_Usuario u
INNER JOIN dbo.Crm_UGrupo ug ON ug.nId_Usuario = u.nId_Usuario
INNER JOIN dbo.Crm_Grupo g ON g.nId_Grupo = ug.nId_Grupo
WHERE u.nId_Usuario = @IdUsuario
AND g.nid_cliente = @IdCliente
AND u.bEstado = 1
AND ug.bEstado = 1
AND ug.bActivo = 1
AND g.bEstado = 1;
""";
}
