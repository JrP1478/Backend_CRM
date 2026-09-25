using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarOpeEstRepository
    {
        Task<IQueryable<Crm_DocxCobrarOpeEst>> Query();
        IQueryable<Crm_DocxCobrarOpeEst> GetGestionesEstadoCarteraDeudor(int nId_Cliente, int nId_Cartera, int nId_PersDeudor);
        IQueryable<Crm_DocxCobrarOpeEst> GetGestionesEstadoCarteraDeudorHistoricas(int nId_Cliente, int nId_Cartera, int nId_PersDeudor);
    }
}