using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_OficinaCrmRepository : ICrm_OficinaCrmRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_OficinaCrm> _dbSet;

        public Crm_OficinaCrmRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_OficinaCrm>();
        }

        public async Task<IQueryable<Crm_OficinaCrm>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}