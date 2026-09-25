using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersDireccRepository
    {
        Task<Crm_PersDirecc> GetDireccionByIdDireccionAsync(int nId_PersDirecc);
        IQueryable<Crm_PersDirecc> GetDireccionByIdDireccion(int nId_PersDirecc);
        Task<IQueryable<Crm_PersDirecc>> Query();
        IQueryable<Crm_PersDirecc> GetGestionesDireccionesAsync(Crm_PersDirecc Crm_PersDirecc);
        Task<Crm_PersDirecc> AddAsync(Crm_PersDirecc Crm_PersDirecc);
        Task<Crm_PersDirecc> UpdateAsync(Crm_PersDirecc Crm_PersDirecc);
    }
}