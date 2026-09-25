using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_CampanaDiscadorRepository
    {
        Task<IQueryable<Crm_CampanaDiscador>> Query();
    }
}