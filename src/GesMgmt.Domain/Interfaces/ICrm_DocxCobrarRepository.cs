using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarRepository
    {
        Task<IQueryable<Crm_DocxCobrar>> Query();
        Task<IQueryable<Crm_DocxCobrar>> GetGestionesAsync(Crm_DocxCobrar Crm_DocxCobrar);
        Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarActivosAsync(int nId_Cliente, int nId_PersDeudor);
        Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByIdClienteAndIdDeudorAsync(int nId_Cliente, int nId_PersDeudor);
        Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarActivosByIdClienteAsync(int nId_Cliente);
        Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByIdClienteAsync(int nId_Cliente);
        Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByNroDocumentoAsync(string letra, int nId_Cliente, string cDoc_Numero);
        Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByClienteAndCarteraAsync(int nId_Cliente, int nId_Cartera);
        Task<Crm_DocxCobrar> GetDocxCobByClienteAndDeudorActivoAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor);
    }
}