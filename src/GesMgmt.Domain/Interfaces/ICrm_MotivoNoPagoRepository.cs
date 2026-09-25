using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_MotivoNoPagoRepository
    {
        Task<IQueryable<Crm_MotivoNoPago>> Query();
        Task<IQueryable<Crm_MotivoNoPago>> MotivoNoPagoByIdClienteAsync(int nId_Cliente);
    }
}