using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_UGrupoRepository : ICrm_UGrupoRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_UGrupo> _dbSet;

        public Crm_UGrupoRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_UGrupo>();
        }

        public async Task<IQueryable<Crm_UGrupo>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_UGrupo> ByIdAsync(int nId_UGrupo)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_UGrupo == nId_UGrupo).FirstOrDefaultAsync();
        }

        public async Task<Crm_UGrupo> AddAsync(Crm_UGrupo Crm_UGrupo)
        {
            await _dbSet.AddAsync(Crm_UGrupo);
            return Crm_UGrupo;
        }

        public async Task<Crm_UGrupo> UpdateAsync(Crm_UGrupo Crm_UGrupo)
        {
            _dbSet.Update(Crm_UGrupo);
            return Crm_UGrupo;
        }

        public async Task<IQueryable<Crm_UGrupo>> GetUGruposActivo()
        {
            return _dbSet
                .Where(ug => ug.bEstado == true)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_UGrupo>> GetUGruposByIdUsuarioAsync(int idUsuario)
        {
            return _dbSet
                .Where(ug => ug.nId_Usuario == idUsuario)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_UGrupo>> GetUGruposActivosByIdUsuarioAsync(int idUsuario)
        {
            return _dbSet
                .Where(ug => ug.nId_Usuario == idUsuario && ug.bEstado == true)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_UGrupo>> GetUGruposInactivosByIdUsuarioAsync(int idUsuario)
        {
            return _dbSet
                .Where(ug => ug.nId_Usuario == idUsuario && ug.bEstado == false)
                .AsNoTracking();
        }

    }
}