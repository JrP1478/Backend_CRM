using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_ConfigSistemaRepository
    {
        Task<IQueryable<Crm_ConfigSistema>> Query();
        Task<Crm_ConfigSistema> GetConfiguracionSistemaByCodigoTablaAsync(int nCodTabla, string cLlave);
    }
}