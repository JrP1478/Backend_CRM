using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DivisionalRepository
    {
        Task<IQueryable<Crm_Divisional>> Query();
    }
}