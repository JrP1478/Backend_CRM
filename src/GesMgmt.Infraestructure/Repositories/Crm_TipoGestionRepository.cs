using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_TipoGestionRepository : ICrm_TipoGestionRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_TipoGestion> _dbSet;

        public Crm_TipoGestionRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_TipoGestion>();
        }

        public async Task<IQueryable<Crm_TipoGestion>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}