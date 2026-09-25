using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarParamRepository
    {
        Task<IQueryable<Crm_DocxCobrarParam>> Query();
        Task<IQueryable<Crm_DocxCobrarParam>> GetGestionesParamByIdCarteraAsync(int nId_Cartera);
        IQueryable<Crm_DocxCobrarParam> GetGestionesParamAsync(Crm_DocxCobrarParam Crm_DocxCobrarParam);
    }
}