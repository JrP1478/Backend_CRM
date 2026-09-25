using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PerfilRepository
    {
        Task<IQueryable<Crm_Perfil>> Query();
        Task<Crm_Perfil> ByIdAsync(int nId_Perfil);
        Task<int> GetMaxIdPerfilAsync();
        Task<Crm_Perfil> AddAsync(Crm_Perfil Crm_Perfil);
        Task<Crm_Perfil> UpdateAsync(Crm_Perfil Crm_Perfil);
    }
}