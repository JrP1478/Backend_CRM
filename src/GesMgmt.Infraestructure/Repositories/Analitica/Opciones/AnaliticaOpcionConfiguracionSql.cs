using GesMgmt.Application.Interfaces.Analitica;
using GesMgmt.Application.Utils.Analitica;
using GesMgmt.Domain.Constants.Analitica;
using GesMgmt.Domain.Entities.Analitica;
using GesMgmt.Domain.Interfaces.Analitica;

namespace GesMgmt.Infraestructure.Repositories.Analitica;

public static class AnaliticaOpcionConfiguracionSql
{
    public const string Exists = """
        SELECT CASE
            WHEN EXISTS
            (
                SELECT 1
                FROM acceso_analitica.configuracion_opcion
                WHERE id_opcion = @IdOpcion
            ) THEN 1
            ELSE 0
        END;
        """;

    public const string GetAll = """
        SELECT
            id_opcion AS IdOpcion,
            codigo_opcion AS CodigoOpcion,
            nombre_opcion AS NombreOpcion
        FROM acceso_analitica.configuracion_opcion
        WHERE es_activo = 1
        ORDER BY id_opcion;
        """;

    public const string Update = """
        UPDATE acceso_analitica.configuracion_opcion
        SET
            codigo_opcion = @CodigoOpcion,
            nombre_opcion = @NombreOpcion,
            es_activo = @EsActivo,
            actualizado_por = @IdUsuario,
            fecha_actualizacion = SYSUTCDATETIME()
        WHERE id_opcion = @IdOpcion;
        """;

    public const string InsertMissing = """
        INSERT INTO acceso_analitica.configuracion_opcion
        (
            id_opcion,
            codigo_opcion,
            nombre_opcion,
            es_activo,
            creado_por,
            fecha_creacion
        )
        SELECT
            @IdOpcion,
            @CodigoOpcion,
            @NombreOpcion,
            @EsActivo,
            @IdUsuario,
            SYSUTCDATETIME()
        WHERE NOT EXISTS
        (
            SELECT 1
            FROM acceso_analitica.configuracion_opcion WITH (UPDLOCK, HOLDLOCK)
            WHERE id_opcion = @IdOpcion
        );
        """;

}
