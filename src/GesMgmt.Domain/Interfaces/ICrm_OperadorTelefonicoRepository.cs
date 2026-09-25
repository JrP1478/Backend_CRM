using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_OperadorTelefonicoRepository
    {
        Task<IQueryable<Crm_OperadorTelefonico>> Query();
    }
}