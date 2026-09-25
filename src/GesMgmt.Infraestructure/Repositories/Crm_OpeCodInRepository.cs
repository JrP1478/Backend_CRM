using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_OpeCodInRepository : ICrm_OpeCodInRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_OpeCodIn> _dbSet;

        public Crm_OpeCodInRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_OpeCodIn>();
        }

        public async Task<IQueryable<Crm_OpeCodIn>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}