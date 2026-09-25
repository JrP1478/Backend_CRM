using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_ZonaCarteraRepository
    {
        Task<IQueryable<Crm_ZonaCartera>> Query();
        Task<Crm_ZonaCartera> GetZonaCarteraByIdClienteAsync(int nId_Cliente);
        Task<IQueryable<Crm_ZonaCartera?>> GetZonasCarterasByIdClienteAsync(int nId_Cliente);
    }
}