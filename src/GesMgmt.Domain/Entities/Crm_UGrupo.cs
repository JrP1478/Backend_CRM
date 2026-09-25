
namespace GesMgmt.Domain.Entities
{
    public class Crm_UGrupo
    {
        public int nId_UGrupo { get; set; }
        public int? nId_Usuario { get; set; }
        public Crm_Usuario Crm_Usuario { get; set; }
        public int? nId_Grupo { get; set; }
        public Crm_Grupo Crm_Grupo { get; set; }
        public DateTime? dUGrupo_FecIni { get; set; }
        public DateTime? dUGrupo_FecFin { get; set; }
        public bool? bEstado { get; set; }
        public bool? bActivo { get; set; }
        public bool? bGestion { get; set; }
    }
}