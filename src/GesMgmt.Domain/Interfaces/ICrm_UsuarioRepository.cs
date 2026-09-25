using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_UsuarioRepository
    {
        Task<IQueryable<Crm_Usuario>> Query();
        Task<Crm_Usuario> GetByIdAsync(int nId_Usuario);
        Task<Crm_Usuario> GetLoginUsuarioAsync(string cUsr_Login, string cUsr_Pass);
        Task<Crm_Usuario> GetByUsuarioAsync(string cUsr_Login);
        Task<IQueryable<Crm_Usuario>> GetUsuariosActivos();
        Task<Crm_Usuario> GetByUsuarioByNroDocumentoAsync(string cUsr_NroDoc);
        Task<Crm_Usuario> GetByUsuarioByAnexoAsync(string cUsr_Anexo);
        Task<Crm_Usuario> GetByUsuarioByLoginAsync(string cUsr_Login);
        Task<Crm_Usuario> AddAsync(Crm_Usuario Crm_Usuario);
        Task<Crm_Usuario> UpdateAsync(Crm_Usuario Crm_Usuario);
        Task<Crm_Usuario> UpdateIntentoLoginAsync(string cUsr_Login);
        Task<Crm_Usuario> UpdateIntentoZeroLoginAsync(string cUsr_Login);
    }
}