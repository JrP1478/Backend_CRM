using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PerfilOpcionRepository
    {
        Task<IQueryable<Crm_PerfilOpcion>> Query();
        Task<Crm_PerfilOpcion> ByIdAsync(int nId_PerfilOpcion);
        Task<Crm_PerfilOpcion> GetPerfilOpcionIdAsync(int nId_Perfil, int nId_Opcion);
        Task<IQueryable<Crm_PerfilOpcion>> GetOpcionesByIdPerfilAsync(int nId_Perfil);
        Task<IQueryable<Crm_PerfilOpcion>> GetOpcionesByIdPerfilActivoAsync(int nId_Perfil);
        Task<Crm_PerfilOpcion> AddAsync(Crm_PerfilOpcion Crm_PerfilOpcion);
        Task<Crm_PerfilOpcion> UpdateAsync(Crm_PerfilOpcion Crm_PerfilOpcion);
    }
}