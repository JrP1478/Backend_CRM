using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersTelefOpeRepository : ICrm_PersTelefOpeRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersTelefOpe> _dbSet;

        public Crm_PersTelefOpeRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersTelefOpe>();
        }

        public async Task<IQueryable<Crm_PersTelefOpe>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_PersTelefOpe> GetResultadosTelefono()
        {
            return _dbSet
                .AsNoTracking()
                .Where(p => p.bEstado == true);
        }
    }
}