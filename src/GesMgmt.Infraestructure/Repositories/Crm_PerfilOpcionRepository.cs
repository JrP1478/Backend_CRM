using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PerfilOpcionRepository : ICrm_PerfilOpcionRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PerfilOpcion> _dbSet;

        public Crm_PerfilOpcionRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PerfilOpcion>();
        }

        public async Task<IQueryable<Crm_PerfilOpcion>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PerfilOpcion> ByIdAsync(int nId_PerfilOpcion)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_PerfilOpcion == nId_PerfilOpcion).FirstOrDefaultAsync();
        }

        public async Task<Crm_PerfilOpcion> GetPerfilOpcionIdAsync(int nId_Perfil, int nId_Opcion)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_Perfil == nId_Perfil && p.nId_Opcion == nId_Opcion).FirstOrDefaultAsync();
        }

        public async Task<IQueryable<Crm_PerfilOpcion>> GetOpcionesByIdPerfilAsync(int nId_Perfil)
        {
            return _dbSet.AsNoTracking().Where(o => o.nId_Perfil == nId_Perfil);
        }

        public async Task<IQueryable<Crm_PerfilOpcion>> GetOpcionesByIdPerfilActivoAsync(int nId_Perfil)
        {
            return _dbSet.AsNoTracking().Where(o => o.nId_Perfil == nId_Perfil && o.bEstado == true);
        }

        public async Task<Crm_PerfilOpcion> AddAsync(Crm_PerfilOpcion Crm_PerfilOpcion)
        {
            await _dbSet.AddAsync(Crm_PerfilOpcion);
            return Crm_PerfilOpcion;
        }

        public async Task<Crm_PerfilOpcion> UpdateAsync(Crm_PerfilOpcion Crm_PerfilOpcion)
        {
            _dbSet.Update(Crm_PerfilOpcion);
            return Crm_PerfilOpcion;
        }
    }
}