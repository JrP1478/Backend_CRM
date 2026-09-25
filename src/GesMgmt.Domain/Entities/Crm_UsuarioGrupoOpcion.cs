
namespace GesMgmt.Domain.Entities
{
    public class Crm_UsuarioGrupoOpcion
    {
        public int nId_UsuarioGrupoOpcion { get; set; }
        public int nId_Usuario { get; set; }
        public Crm_Usuario Crm_Usuario { get; set; }
        public int nId_Grupo { get; set; }
        public Crm_Grupo Crm_Grupo { get; set; }
        public int nId_Opcion { get; set; }
        public Crm_Opcion Crm_Opcion { get; set; }
        public bool? bConsultar { get; set; }
        public bool? bInsertar { get; set; }
        public bool? bEditar { get; set; }
        public bool? bEliminar { get; set; }
        public bool? bExportar { get; set; }
        public bool bEstado { get; set; }
        public int nCrea { get; set; }
        public DateTime dFechaCrea { get; set; }
        public int? nModifica { get; set; }
        public DateTime? dFechaModifica { get; set; }
    }
}