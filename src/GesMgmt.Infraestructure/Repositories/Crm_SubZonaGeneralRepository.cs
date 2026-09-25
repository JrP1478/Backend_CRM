using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_SubZonaGeneralRepository : ICrm_SubZonaGeneralRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_SubZonaGeneral> _dbSet;

        public Crm_SubZonaGeneralRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_SubZonaGeneral>();
        }

        public async Task<IQueryable<Crm_SubZonaGeneral>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}