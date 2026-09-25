using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_MaeTablaRepository : ICrm_MaeTablaRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_MaeTabla> _dbSet;

        public Crm_MaeTablaRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_MaeTabla>();
        }

        public async Task<IQueryable<Crm_MaeTabla>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}