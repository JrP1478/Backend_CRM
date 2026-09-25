using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_OficinaCrmRepository
    {
        Task<IQueryable<Crm_OficinaCrm>> Query();
    }
}