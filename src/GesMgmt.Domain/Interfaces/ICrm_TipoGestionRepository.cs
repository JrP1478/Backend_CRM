using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_TipoGestionRepository
    {
        Task<IQueryable<Crm_TipoGestion>> Query();
    }
}