using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_CarteraRepository
    {
        Task<IQueryable<Crm_Cartera>> Query();
        Task<Crm_Cartera> GetCarteraByIdClienteIdCarteraAsync(int nId_Cliente, int nId_Cartera);
        Task<IQueryable<Crm_Cartera?>> GetCarteraByClienteCarteraAsync(int nId_Cliente, int nId_Cartera);
        Task<IQueryable<Crm_Cartera?>> GetCarterasByIdClienteActivoAsync(int nId_Cliente);
        Task<IQueryable<Crm_Cartera?>> GetCarterasByIdClienteAsync(int nId_Cliente);
        Task<IQueryable<Crm_Cartera>> GetCarterasParametrosByIdClienteAnnioAsync(int nId_Cliente, int Annio);
        Task<IQueryable<Crm_Cartera?>> GetCarterasByIdClienteAndIdCarteraAsync(int nId_Cliente, int nId_Cartera);
    }
}