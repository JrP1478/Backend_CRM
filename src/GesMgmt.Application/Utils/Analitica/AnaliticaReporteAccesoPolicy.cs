using GesMgmt.Domain.Constants.Analitica;

namespace GesMgmt.Application.Utils.Analitica;

public static class AnaliticaReporteAccesoPolicy
{
    // El BI 27 selecciona la cartera después de entrar al módulo.
    // Solo omite el alcance global de la opción; los grupos de cada cartera
    // continúan determinando qué publicaciones puede seleccionar el usuario.
    public static bool OmiteValidacionAlcanceGrupoOpcion(int idOpcion) =>
        idOpcion == AnaliticaOpcionIds.GestionIntegralCobranza;
}
