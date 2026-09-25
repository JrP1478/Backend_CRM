using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DetallePersTelefRepository
    {
        Task<IQueryable<Crm_DetallePersTelef>> Query();
        IQueryable<Crm_DetallePersTelef> GetDetalleTelefonosAsync(Crm_DetallePersTelef Crm_DetallePersTelef);
        Task<Crm_DetallePersTelef> GetDetalleTelefonoSearchAsync(int nId_Cliente, int nId_PersTelef);
        Task<Crm_DetallePersTelef> AddAsync(Crm_DetallePersTelef Crm_DetallePersTelef);
    }
}