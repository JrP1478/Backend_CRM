using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_CampanaDiscadorRepository : ICrm_CampanaDiscadorRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_CampanaDiscador> _dbSet;

        public Crm_CampanaDiscadorRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_CampanaDiscador>();
        }

        public async Task<IQueryable<Crm_CampanaDiscador>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}