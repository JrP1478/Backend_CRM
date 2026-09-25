
namespace GesMgmt.Domain.Entities
{
    public class Crm_DetallePersTelef
    {
        public int nId_DetallePersTelef { get; set; }
        public int nId_PersTelef { get; set; }
        public Crm_PersTelef Crm_PersTelef { get; set; }
        public int nId_Cliente { get; set; }
        public Crm_Cliente Crm_Cliente { get; set; }
        public DateTime? dFec_Registro { get; set; }
        public DateTime? dFec_Actualiza { get; set; }
        public int? nId_Fuente { get; set; }
        public int? nId_UsuReg { get; set; }
        public bool? bBase { get; set; }
        public bool? bestado { get; set; }
    }
}
