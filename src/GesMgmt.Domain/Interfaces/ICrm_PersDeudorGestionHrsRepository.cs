using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersDeudorGestionHrsRepository
    {
        Task<IQueryable<Crm_PersDeudorGestionHrs>> Query();
        IQueryable<Crm_PersDeudorGestionHrs> GetHorarioGestionTelefono();
    }
}