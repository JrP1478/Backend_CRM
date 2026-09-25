using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DiscadorRepository : ICrm_DiscadorRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Discador> _dbSet;

        public Crm_DiscadorRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Discador>();
        }

        public async Task<IQueryable<Crm_Discador>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}