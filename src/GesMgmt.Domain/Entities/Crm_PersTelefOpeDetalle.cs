
namespace GesMgmt.Domain.Entities
{
    public class Crm_PersTelefOpeDetalle
    {
        public int nId_PersTelefOpeDet { get; set; }
        public int? nId_PersTelef { get; set; }
        public Crm_PersTelef Crm_PersTelef { get; set; }
        public int? nId_PersTelefOpe { get; set; }
        public Crm_PersTelefOpe Crm_PersTelefOpe { get; set; }
        public DateTime? dFec_PerstelefOpe { get; set; }
        public int? nId_Usuario { get; set; }
    }
}