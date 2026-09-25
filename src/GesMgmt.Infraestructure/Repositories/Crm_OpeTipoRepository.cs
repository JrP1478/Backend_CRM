using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_OpeTipoRepository : ICrm_OpeTipoRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_OpeTipo> _dbSet;

        public Crm_OpeTipoRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_OpeTipo>();
        }

        public async Task<IQueryable<Crm_OpeTipo>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}