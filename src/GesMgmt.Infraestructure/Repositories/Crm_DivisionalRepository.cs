using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DivisionalRepository : ICrm_DivisionalRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Divisional> _dbSet;

        public Crm_DivisionalRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Divisional>();
        }

        public async Task<IQueryable<Crm_Divisional>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}