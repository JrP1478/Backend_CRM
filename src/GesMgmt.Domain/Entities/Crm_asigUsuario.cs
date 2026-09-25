
namespace GesMgmt.Domain.Entities
{
    public class Crm_asigUsuario
    {
        public int nid_asignacion { get; set; }
        public int? nid_usuario { get; set; }
        public int? nid_cliente { get; set; }
        public string? zona { get; set; }
        public bool? bestado { get; set; }
    }
}