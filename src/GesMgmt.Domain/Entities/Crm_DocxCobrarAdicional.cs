
namespace GesMgmt.Domain.Entities
{
    public class Crm_DocxCobrarAdicional
    {
        public int nId_DocxCobrarAd { get; set; }

        public int? nId_Cliente { get; set; }
        public int? nId_Cartera { get; set; }
        public int? nId_DocxCobrar { get; set; }
        public int? nId_PersDeudor { get; set; }

        public virtual Crm_Cliente Crm_Cliente { get; set; }
        public virtual Crm_Cartera Crm_Cartera { get; set; }
        public virtual Crm_DocxCobrar Crm_DocxCobrar { get; set; }
        public virtual Crm_PersDeudor Crm_PersDeudor { get; set; }

        public string? adParam01 { get; set; }
        public string? adParam02 { get; set; }
        public string? adParam03 { get; set; }
        public string? adParam04 { get; set; }
        public string? adParam05 { get; set; }

        public DateTime? dFecRegistro { get; set; }
        public string? adParam06 { get; set; }
        public string? adParam07 { get; set; }
        public string? adParam08 { get; set; }
        public string? adParam09 { get; set; }
        public string? adParam10 { get; set; }
        public string? adParam11 { get; set; }
        public string? adParam12 { get; set; }
        public string? adParam13 { get; set; }
        public string? adParam14 { get; set; }
        public string? adParam15 { get; set; }
    }
}