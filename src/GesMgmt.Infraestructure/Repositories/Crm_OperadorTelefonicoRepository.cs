using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_OperadorTelefonicoRepository : ICrm_OperadorTelefonicoRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_OperadorTelefonico> _dbSet;

        public Crm_OperadorTelefonicoRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_OperadorTelefonico>();
        }

        public async Task<IQueryable<Crm_OperadorTelefonico>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}