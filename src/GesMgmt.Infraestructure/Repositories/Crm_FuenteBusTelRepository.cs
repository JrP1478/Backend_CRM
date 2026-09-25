using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_FuenteBusTelRepository : ICrm_FuenteBusTelRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_FuenteBusTel> _dbSet;

        public Crm_FuenteBusTelRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_FuenteBusTel>();
        }

        public async Task<IQueryable<Crm_FuenteBusTel>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}
