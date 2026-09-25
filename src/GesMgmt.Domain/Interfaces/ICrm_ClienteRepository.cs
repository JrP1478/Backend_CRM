using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_ClienteRepository
    {
        Task<IQueryable<Crm_Cliente>> Query();
        Task<IQueryable<Crm_Cliente>> ClientesActivosAsync();
    }
}