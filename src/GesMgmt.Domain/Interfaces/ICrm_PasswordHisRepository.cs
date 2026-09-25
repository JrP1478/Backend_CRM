using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PasswordHisRepository
    {
        Task<IQueryable<Crm_PasswordHis>> Query();
        Task<Crm_PasswordHis> ByIdAsync(int nId_PasswordHis);
        Task<Crm_PasswordHis> ByClavePorFechaAsync(int nId_Usuario, string cUsr_Pass, DateTime dFecRegistro);
        Task<Crm_PasswordHis> AddAsync(Crm_PasswordHis Crm_PasswordHis);
        Task<Crm_PasswordHis> UpdateAsync(Crm_PasswordHis Crm_PasswordHis);
    }
}