using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

public static class AnaliticaAlcanceGrupoOpcionSql
{
    public const string TieneAlgunAlcance = """
        SELECT COUNT(1)
        FROM acceso_analitica.alcance_opcion_grupo
        WHERE id_opcion = @IdOpcion;
        """;

    public const string GetGroups = """
        SELECT id_grupo_sisges
        FROM acceso_analitica.alcance_opcion_grupo
        WHERE id_opcion = @IdOpcion
          AND es_activo = 1
        ORDER BY id_grupo_sisges;
        """;

    public const string GetScopes = """
        WITH requested_options AS
        (
            SELECT DISTINCT TRY_CONVERT(INT, [value]) AS id_opcion
            FROM OPENJSON(@OptionIdsJson)
            WHERE TRY_CONVERT(INT, [value]) > 0
        )
        SELECT
            scope.id_opcion AS IdOpcion,
            scope.id_grupo_sisges AS IdGrupoSisges,
            scope.es_activo AS EsActivo
        FROM acceso_analitica.alcance_opcion_grupo AS scope
        INNER JOIN requested_options AS requested
            ON requested.id_opcion = scope.id_opcion
        ORDER BY
            scope.id_opcion,
            scope.id_grupo_sisges;
        """;

    public const string Replace = """
        DECLARE @RequestedGroups TABLE
        (
            id_grupo_sisges INT NOT NULL PRIMARY KEY
        );

        INSERT INTO @RequestedGroups (id_grupo_sisges)
        SELECT DISTINCT TRY_CONVERT(INT, [value])
        FROM OPENJSON(@GroupIdsJson)
        WHERE TRY_CONVERT(INT, [value]) > 0;

        UPDATE acceso_analitica.alcance_opcion_grupo
        SET
            es_activo = 0,
            actualizado_por = @IdUsuario,
            fecha_actualizacion = SYSUTCDATETIME()
        WHERE id_opcion = @IdOpcion
          AND es_activo = 1;

        UPDATE meta
        SET
            meta.es_activo = 1,
            meta.actualizado_por = @IdUsuario,
            meta.fecha_actualizacion = SYSUTCDATETIME()
        FROM acceso_analitica.alcance_opcion_grupo AS meta
        INNER JOIN @RequestedGroups AS requested
            ON requested.id_grupo_sisges = meta.id_grupo_sisges
        WHERE meta.id_opcion = @IdOpcion;

        INSERT INTO acceso_analitica.alcance_opcion_grupo
        (
            id_opcion,
            id_grupo_sisges,
            es_activo,
            creado_por,
            fecha_creacion,
            actualizado_por,
            fecha_actualizacion
        )
        SELECT
            @IdOpcion,
            requested.id_grupo_sisges,
            1,
            @IdUsuario,
            SYSUTCDATETIME(),
            @IdUsuario,
            SYSUTCDATETIME()
        FROM @RequestedGroups AS requested
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM acceso_analitica.alcance_opcion_grupo AS meta
                WITH (UPDLOCK, HOLDLOCK)
            WHERE meta.id_opcion = @IdOpcion
              AND meta.id_grupo_sisges = requested.id_grupo_sisges
        );
        """;
}
