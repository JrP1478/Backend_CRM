using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_TablaCampoGeneralRepository
    {
        Task<IQueryable<Crm_TablaCampoGeneral>> Query();
        Task<IQueryable<Crm_TablaCampoGeneral>> GetCabeceraGestionesAdicionalAsync(Crm_TablaCampoGeneral Crm_TablaCampoGeneral);
    }
}