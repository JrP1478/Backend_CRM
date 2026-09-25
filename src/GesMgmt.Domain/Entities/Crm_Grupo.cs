
namespace GesMgmt.Domain.Entities
{
    public class Crm_Grupo
    {
        public int nId_Grupo { get; set; }
        public string? cNombre_Grupo { get; set; }
        public string? cSigla_Grupo { get; set; }
        public bool? bEstado { get; set; }
        public int? nCant_Grupo { get; set; }
        public int? nid_cliente { get; set; }
        public Crm_Cliente Crm_Cliente { get; set; }
    }
}