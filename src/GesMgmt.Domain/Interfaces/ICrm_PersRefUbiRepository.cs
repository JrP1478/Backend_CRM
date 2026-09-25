using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersRefUbiRepository
    {
        Task<IQueryable<Crm_PersRefUbi>> Query();
        IQueryable<Crm_PersRefUbi> GetUbicacionesTelefono();
        IQueryable<Crm_PersRefUbi> GetUbicacionesDireccion();
    }
}