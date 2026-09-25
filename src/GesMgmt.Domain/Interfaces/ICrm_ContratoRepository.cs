using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_ContratoRepository
    {
        Task<IQueryable<Crm_Contrato>> Query();
    }
}