
namespace GesMgmt.Domain.Entities
{
    public class Crm_OficinaCrm
    {
        public int nid_OficinaCrm { get; set; }
        public string? cNombre_Oficina { get; set; }
        public string? cDireccion { get; set; }
        public int? nId_Usuario { get; set; }
        public Crm_Usuario Crm_Usuario { get; set; }
        public bool? bLimaProv { get; set; }
        public int? nid_usuarioAsistente { get; set; }
        public int? nid_cliente { get; set; }
        public int? nId_ZonaGen { get; set; }
        public int? nId_subZonaGen { get; set; }
    }
}