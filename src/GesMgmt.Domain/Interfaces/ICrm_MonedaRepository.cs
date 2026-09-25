using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_MonedaRepository
    {
        Task<IQueryable<Crm_Moneda>> Query();
    }
}