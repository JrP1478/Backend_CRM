using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersEmailOpeRepository : ICrm_PersEmailOpeRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersEmailOpe> _dbSet;

        public Crm_PersEmailOpeRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersEmailOpe>();
        }

        public async Task<IQueryable<Crm_PersEmailOpe>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}