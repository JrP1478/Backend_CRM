using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersDeudorParamRepository
    {
        Task<IQueryable<Crm_PersDeudorParam>> Query();
        Task<IQueryable<Crm_PersDeudorParam?>> GetDeudorParamByIdDeudorAsync(int nId_PersDeudor);
        Task<IQueryable<Crm_PersDeudorParam?>> GetDeudorParamAsync();
    }
}