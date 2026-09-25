using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_OpcionRepository
    {
        Task<IQueryable<Crm_Opcion>> Query();
        Task<Crm_Opcion> ByIdAsync(int nId_Opcion);
        Task<IQueryable<Crm_Opcion>> QueryByIdPadre(int nId_OpcionPadre);
        Task<Crm_Opcion> ByIdPadreAsync(int nId_OpcionPadre);
        Task<Crm_Opcion> AddAsync(Crm_Opcion Crm_Opcion);
        Task<Crm_Opcion> UpdateAsync(Crm_Opcion Crm_Opcion);
    }
}