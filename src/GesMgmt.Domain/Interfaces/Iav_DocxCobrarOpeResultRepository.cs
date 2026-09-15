using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface Iav_DocxCobrarOpeResultRepository
    {
        Task<IQueryable<av_DocxCobrarOpeResult>> Query();
        Task<IQueryable<av_DocxCobrarOpeResult?>> GetReporteCasosByClienteAndCarterasActivoAsync(int nId_Cliente, int nId_Cartera);
        Task<av_DocxCobrarOpeResult> GetReporteCasosByIdAsync(int nId_DocxCobrarOpeResult);
        Task<av_DocxCobrarOpeResult> AddAsync(av_DocxCobrarOpeResult av_DocxCobrarOpeResult);
        Task<av_DocxCobrarOpeResult> UpdateAsync(av_DocxCobrarOpeResult av_DocxCobrarOpeResult);
    }
}