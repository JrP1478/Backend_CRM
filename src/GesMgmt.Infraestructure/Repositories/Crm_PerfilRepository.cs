using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PerfilRepository : ICrm_PerfilRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Perfil> _dbSet;

        public Crm_PerfilRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Perfil>();
        }

        public async Task<IQueryable<Crm_Perfil>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_Perfil> ByIdAsync(int nId_Perfil)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nid_perfil == nId_Perfil).FirstOrDefaultAsync();
        }

        public async Task<int> GetMaxIdPerfilAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .MaxAsync(p => (int?)p.nid_perfil) ?? 0;
        }

        public async Task<Crm_Perfil> AddAsync(Crm_Perfil Crm_Perfil)
        {
            await _dbSet.AddAsync(Crm_Perfil);
            return Crm_Perfil;
        }

        public async Task<Crm_Perfil> UpdateAsync(Crm_Perfil Crm_Perfil)
        {
            _dbSet.Update(Crm_Perfil);
            return Crm_Perfil;
        }
    }
}