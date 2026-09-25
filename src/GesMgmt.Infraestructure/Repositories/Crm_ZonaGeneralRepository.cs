using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ZonaGeneralRepository : ICrm_ZonaGeneralRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_ZonaGeneral> _dbSet;

        public Crm_ZonaGeneralRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_ZonaGeneral>();
        }

        public async Task<IQueryable<Crm_ZonaGeneral>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}