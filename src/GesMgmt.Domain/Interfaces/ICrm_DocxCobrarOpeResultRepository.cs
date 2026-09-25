using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarOpeResultRepository
    {
        Task<IQueryable<Crm_DocxCobrarOpeResult>> Query();
        Task<IQueryable<Crm_DocxCobrarOpeResult?>> GetReporteCasosByClienteAndCarterasActivoAsync(int nId_Cliente, int nId_Cartera);
        Task<Crm_DocxCobrarOpeResult> GetReporteCasosByIdAsync(int nId_DocxCobrarOpeResult);
        Task<Crm_DocxCobrarOpeResult> AddAsync(Crm_DocxCobrarOpeResult Crm_DocxCobrarOpeResult);
        Task<Crm_DocxCobrarOpeResult> UpdateAsync(Crm_DocxCobrarOpeResult Crm_DocxCobrarOpeResult);
    }
}