using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_FuenteBusTelRepository
    {
        Task<IQueryable<Crm_FuenteBusTel>> Query();
    }
}