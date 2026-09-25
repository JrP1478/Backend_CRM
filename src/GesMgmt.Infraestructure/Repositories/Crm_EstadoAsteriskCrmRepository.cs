using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_EstadoAsteriskCrmRepository : ICrm_EstadoAsteriskCrmRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_EstadoAsteriskCrm> _dbSet;

        public Crm_EstadoAsteriskCrmRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_EstadoAsteriskCrm>();
        }

        public async Task<IQueryable<Crm_EstadoAsteriskCrm>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}