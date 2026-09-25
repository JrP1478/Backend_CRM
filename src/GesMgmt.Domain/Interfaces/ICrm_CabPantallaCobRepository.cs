using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_CabPantallaCobRepository
    {
        Task<IQueryable<Crm_CabPantallaCob>> Query();
        IQueryable<Crm_CabPantallaCob> GetCabeceraGestionesAsync(Crm_CabPantallaCob Crm_CabPantallaCob);
    }
}