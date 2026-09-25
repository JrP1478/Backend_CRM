using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarOpeGesRepository
    {
        Task<IQueryable<Crm_DocxCobrarOpeGes>> Query();
        Task<Crm_DocxCobrarOpeGes> AddAsync(Crm_DocxCobrarOpeGes Crm_DocxCobrarOpeGes);
        Task<Crm_DocxCobrarOpeGes> UpdateAsync(Crm_DocxCobrarOpeGes Crm_DocxCobrarOpeGes);
        Task<IEnumerable<Crm_DocxCobrarOpeGes>> AddRangeAsync(IEnumerable<Crm_DocxCobrarOpeGes> entities);
    }
}