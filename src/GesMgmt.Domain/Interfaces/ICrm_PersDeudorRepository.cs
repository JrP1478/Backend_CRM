using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersDeudorRepository
    {
        Task<IQueryable<Crm_PersDeudor>> Query();
        Task<Crm_PersDeudor> GetDeudorByIdDeudorAsync(int nId_PersDeudor);
        Task<IQueryable<Crm_PersDeudor?>> GetDeudorByDniRucAsync(string letra, string valor);
        Task<IQueryable<Crm_PersDeudor?>> GetDeudoresByIdDeudorAsync(int nId_PersDeudor);
    }
}