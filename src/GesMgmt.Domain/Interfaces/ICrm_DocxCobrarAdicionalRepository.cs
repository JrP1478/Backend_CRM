using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarAdicionalRepository
    {
        Task<IQueryable<Crm_DocxCobrarAdicional>> Query();
        IQueryable<Crm_DocxCobrarAdicional> GetGestionesAdicionalesAsync(Crm_DocxCobrarAdicional Crm_DocxCobrarAdicional);
    }
}