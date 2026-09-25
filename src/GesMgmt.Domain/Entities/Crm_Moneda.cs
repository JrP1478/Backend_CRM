
namespace GesMgmt.Domain.Entities
{
    public class Crm_Moneda //: BaseEntity
    {
        public int nId_Moneda { get; set; }
        public string? cNombre_Moneda { get; set; }
        public string? cSigla_Moneda { get; set; }
        public bool? bEstado { get; set; }
        public string? cAbreviado { get; set; }

        //Relaciones de navegación
        //public ICollection<Crm_DocxCobrar> Crm_DocxCobrars { get; set; }
    }
}