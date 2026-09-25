using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_SubZonaGeneralRepository
    {
        Task<IQueryable<Crm_SubZonaGeneral>> Query();
    }
}