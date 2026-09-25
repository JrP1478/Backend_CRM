using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarOpeRepository
    {
        Task<IQueryable<Crm_DocxCobrarOpe>> Query();
        Task<IQueryable<Crm_DocxCobrarOpe?>> GetGestionesCarteraDeudorAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor, int? nId_PerfilUsuario);
        IQueryable<Crm_DocxCobrarOpe?> GetGestionesCarteraDeudorHistoricas(int nId_Cliente, int nId_Cartera, int nId_PersDeudor);
        Task<Crm_DocxCobrarOpe?> GetDeudorUltimaGestionTipoAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor, int nId_TipoGestion);
        Task<Crm_DocxCobrarOpe?> GetGestionMejorGestionAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor);
        IQueryable<Crm_DocxCobrarOpe?> GetGestionListarGestionesAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor);
        Task<Crm_DocxCobrarOpe> AddAsync(Crm_DocxCobrarOpe Crm_DocxCobrarOpe);
        Task<Crm_DocxCobrarOpe> UpdateAsync(Crm_DocxCobrarOpe Crm_DocxCobrarOpe);
        Task<IQueryable<Crm_DocxCobrarOpe?>> GetGestionesByIdUsuarioToDay(int nId_Cliente, int nId_Usuario);
    }
}