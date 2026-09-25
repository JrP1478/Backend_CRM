using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ContFormTipoCrudRepository : ICrm_ContFormTipoCrudRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_ContFormTipoCrud> _dbSet;

        public Crm_ContFormTipoCrudRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_ContFormTipoCrud>();
        }

        public async Task<IQueryable<Crm_ContFormTipoCrud>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}