using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersTelefOpeDetalleRepository
    {
        Task<IQueryable<Crm_PersTelefOpeDetalle>> Query();
        Task<Crm_PersTelefOpeDetalle> AddAsync(Crm_PersTelefOpeDetalle Crm_PersTelefOpeDetalle);
    }
}