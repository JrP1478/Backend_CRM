using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_UsuarioGrupoOpcionRepository
    {
        Task<IQueryable<Crm_UsuarioGrupoOpcion>> Query();
        Task<IQueryable<Crm_UsuarioGrupoOpcion>> ByIdUsuarioIdGrupoAsync(int nId_Usuario, int nId_Grupo);
        Task<Crm_UsuarioGrupoOpcion> ByIdAsync(int nId_UsuarioGrupoOpcion);
        Task<Crm_UsuarioGrupoOpcion> AddAsync(Crm_UsuarioGrupoOpcion Crm_UsuarioGrupoOpcion);
        Task<Crm_UsuarioGrupoOpcion> UpdateAsync(Crm_UsuarioGrupoOpcion Crm_UsuarioGrupoOpcion);
    }
}