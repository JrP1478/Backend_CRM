using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersTelefOpeRepository
    {
        Task<IQueryable<Crm_PersTelefOpe>> Query();
        IQueryable<Crm_PersTelefOpe> GetResultadosTelefono();
    }
}