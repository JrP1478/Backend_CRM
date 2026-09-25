using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersDireccRepository : ICrm_PersDireccRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersDirecc> _dbSet;

        public Crm_PersDireccRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersDirecc>();
        }

        public async Task<IQueryable<Crm_PersDirecc>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PersDirecc> GetDireccionByIdDireccionAsync(int nId_PersDirecc)
        {
            return await _dbSet
                 .Include(d => d.Crm_Cliente)
                 .Include(tel => tel.Crm_PersDeudor)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(s => s.nId_PersDirecc == nId_PersDirecc);
        }

        public IQueryable<Crm_PersDirecc> GetDireccionByIdDireccion(int nId_PersDirecc)
        {
            return _dbSet
                .AsNoTracking()
                .Where(x => x.nId_PersDirecc == nId_PersDirecc);
        }

        public IQueryable<Crm_PersDirecc> GetGestionesDireccionesAsync(Crm_PersDirecc Crm_PersDirecc)
        {
            var query = _dbSet
                .Include(d => d.Crm_Cliente)
                .Include(d => d.Crm_PersDeudor)
                .AsNoTracking()
                .AsQueryable();

            if (Crm_PersDirecc.nId_Cliente > 0)
                query = query.Where(s => s.nId_Cliente == Crm_PersDirecc.nId_Cliente);

            if (Crm_PersDirecc.nId_PersDeudor > 0)
                query = query.Where(s => s.nId_PersDeudor == Crm_PersDirecc.nId_PersDeudor);

            return query;
        }

        public async Task<Crm_PersDirecc> AddAsync(Crm_PersDirecc Crm_PersDirecc)
        {
            await _dbSet.AddAsync(Crm_PersDirecc);
            return Crm_PersDirecc;
        }

        public async Task<Crm_PersDirecc> UpdateAsync(Crm_PersDirecc Crm_PersDirecc)
        {
            _dbSet.Update(Crm_PersDirecc);
            return Crm_PersDirecc;
        }
    }
}