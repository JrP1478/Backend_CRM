using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersTelefRepository
    {
        Task<IQueryable<Crm_PersTelef>> Query();
        Task<Crm_PersTelef> GetTelefonoByIdTelefonoAsync(int nId_PersTelef);
        IQueryable<Crm_PersTelef> GetTelefonosAsync(Crm_PersTelef Crm_PersTelef);
        Task<Crm_PersTelef> GetTelefonoNroTelefonoByIdDeudorAsync(string nTelef_Nro, int nId_PersDeudor);
        Task<Crm_PersTelef> GetTelefonoNroTelefonoAsync(string nTelef_Nro);
        Task<Crm_PersTelef> AddAsync(Crm_PersTelef Crm_PersTelef);
        Task<Crm_PersTelef> UpdateAsync(Crm_PersTelef Crm_PersTelef);
        Task<IQueryable<Crm_PersTelef?>> GetDeudorByTelefonoAsync(string letra, string valor);
    }
}