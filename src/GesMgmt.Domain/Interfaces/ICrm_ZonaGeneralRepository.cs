using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_ZonaGeneralRepository
    {
        Task<IQueryable<Crm_ZonaGeneral>> Query();
    }
}