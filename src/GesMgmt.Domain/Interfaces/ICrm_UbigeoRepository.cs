using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_UbigeoRepository
    {
        Task<IQueryable<Crm_Ubigeo>> Query();
        IQueryable<Crm_Ubigeo> GetDepartamentosAsync();
        IQueryable<Crm_Ubigeo> GetProvinciasAsync(int nId_Departamento);
        IQueryable<Crm_Ubigeo> GetDistritosAsync(int nId_Departamento, int nId_Provincias);
    }
}