using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_UGrupoRepository
    {
        Task<IQueryable<Crm_UGrupo>> Query();
        Task<Crm_UGrupo> ByIdAsync(int nId_UGrupo);
        Task<Crm_UGrupo> AddAsync(Crm_UGrupo Crm_UGrupo);
        Task<Crm_UGrupo> UpdateAsync(Crm_UGrupo Crm_UGrupo);
        Task<IQueryable<Crm_UGrupo>> GetUGruposActivo();
        Task<IQueryable<Crm_UGrupo>> GetUGruposByIdUsuarioAsync(int idUsuario);
        Task<IQueryable<Crm_UGrupo>> GetUGruposActivosByIdUsuarioAsync(int idUsuario);
        Task<IQueryable<Crm_UGrupo>> GetUGruposInactivosByIdUsuarioAsync(int idUsuario);
    }
}