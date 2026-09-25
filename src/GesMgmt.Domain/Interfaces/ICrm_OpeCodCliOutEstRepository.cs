using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_OpeCodCliOutEstRepository
    {
        Task<IQueryable<Crm_OpeCodCliOutEst>> Query();
        Task<IQueryable<Crm_OpeCodCliOutEst>> EstadoGestionByIdClienteAsync(int nId_Cliente);
    }
}