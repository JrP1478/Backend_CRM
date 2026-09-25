using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_MonedaRepository : ICrm_MonedaRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Moneda> _dbSet;

        public Crm_MonedaRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Moneda>();
        }

        public async Task<IQueryable<Crm_Moneda>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}