
namespace GesMgmt.Domain.Entities
{
    public class Crm_ZonaGeneral
    {
        public int nId_ZonaGen { get; set; }
        public string cZgn_Nombre { get; set; }
        public int? nId_Usuario { get; set; }
        public Crm_Usuario Crm_Usuario { get; set; }
        public int? nOrdenPresentacion { get; set; }
        public bool? bEstado { get; set; }
        public string? cOfic_Direccion { get; set; }
        public int? nOfic_Ubigeo { get; set; }
        public string? cOfic_TelefMovil01 { get; set; }
        public string? cOfic_TelefMovil02 { get; set; }
        public string? cOfic_TelefFijo01 { get; set; }
    }
}
