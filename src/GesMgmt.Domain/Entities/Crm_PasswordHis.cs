
namespace GesMgmt.Domain.Entities
{
    public class Crm_PasswordHis
    {
        public int nId_PasswordHis { get; set; }
        public DateTime dFecRegistro { get; set; }
        public int nId_Usuario { get; set; }
        public Crm_Usuario Crm_Usuario { get; set; }
        public string cUsr_Pass { get; set; }
        public int nId_UsuarioReg { get; set; }
    }
}