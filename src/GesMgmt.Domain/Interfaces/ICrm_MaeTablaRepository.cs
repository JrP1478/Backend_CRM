using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_MaeTablaRepository
    {
        Task<IQueryable<Crm_MaeTabla>> Query();
    }
}