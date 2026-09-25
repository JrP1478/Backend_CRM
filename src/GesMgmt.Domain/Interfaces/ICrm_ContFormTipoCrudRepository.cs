using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_ContFormTipoCrudRepository
    {
        Task<IQueryable<Crm_ContFormTipoCrud>> Query();
    }
}
