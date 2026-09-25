using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_EstadoAsteriskCrmRepository
    {
        Task<IQueryable<Crm_EstadoAsteriskCrm>> Query();
    }
}