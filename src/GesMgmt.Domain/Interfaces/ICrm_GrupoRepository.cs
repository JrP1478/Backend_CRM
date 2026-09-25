using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_GrupoRepository
    {
        Task<IQueryable<Crm_Grupo>> Query();
        Task<Crm_Grupo> ByIdAsync(int nId_Grupo);
        Task<Crm_Grupo> ByNombreGrupoAsync(string nombreGrupo);
        Task<IQueryable<Crm_Grupo>> GetGruposByCliente(int nId_Cliente);
        Task<IQueryable<Crm_Grupo>> GetGruposActivos();
        Task<Crm_Grupo> AddAsync(Crm_Grupo Crm_Grupo);
        Task<Crm_Grupo> UpdateAsync(Crm_Grupo Crm_Grupo);
    }
}