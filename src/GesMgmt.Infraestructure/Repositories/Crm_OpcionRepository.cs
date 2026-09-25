using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_OpcionRepository : ICrm_OpcionRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Opcion> _dbSet;

        public Crm_OpcionRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Opcion>();
        }

        public async Task<IQueryable<Crm_Opcion>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_Opcion> ByIdAsync(int nId_Opcion)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_Opcion == nId_Opcion).FirstOrDefaultAsync();
        }

        public async Task<IQueryable<Crm_Opcion>> QueryByIdPadre(int nId_OpcionPadre)
        {
            return _dbSet.AsNoTracking().Where(o => o.nId_Opcion == nId_OpcionPadre);
        }

        public async Task<Crm_Opcion> ByIdPadreAsync(int nId_OpcionPadre)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_Opcion == nId_OpcionPadre).FirstOrDefaultAsync();
        }

        public async Task<Crm_Opcion> AddAsync(Crm_Opcion Crm_Opcion)
        {
            await _dbSet.AddAsync(Crm_Opcion);
            return Crm_Opcion;
        }

        public async Task<Crm_Opcion> UpdateAsync(Crm_Opcion Crm_Opcion)
        {
            _dbSet.Update(Crm_Opcion);
            return Crm_Opcion;
        }
    }
}