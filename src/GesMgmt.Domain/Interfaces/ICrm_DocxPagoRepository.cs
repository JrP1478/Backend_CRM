using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxPagoRepository
    {
        Task<IQueryable<Crm_DocxPago>> Query();
        IQueryable<Crm_DocxPago?> GetPagosByIdDeudorAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor);
    }
}