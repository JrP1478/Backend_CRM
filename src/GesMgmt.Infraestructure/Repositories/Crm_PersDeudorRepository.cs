using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersDeudorRepository : ICrm_PersDeudorRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersDeudor> _dbSet;

        public Crm_PersDeudorRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersDeudor>();
        }

        public async Task<IQueryable<Crm_PersDeudor>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PersDeudor> GetDeudorByIdDeudorAsync(int nId_PersDeudor)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_PersDeudor == nId_PersDeudor);
        }

        public async Task<IQueryable<Crm_PersDeudor?>> GetDeudorByDniRucAsync(string letra, string valor)
        {
            if (letra == "R")
            {
                return _dbSet
                .Where(s => s.cPers_RUC == valor)
                .AsNoTracking();
            }
            if (letra == "D")
            {
                return _dbSet
                .Where(s => s.cPers_DNI == valor)
                .AsNoTracking();
            }
            return null;
        }

        public async Task<IQueryable<Crm_PersDeudor?>> GetDeudoresByIdDeudorAsync(int nId_PersDeudor)
        {
            return _dbSet
            .Where(s => s.nId_PersDeudor == nId_PersDeudor)
            .AsNoTracking();
        }
    }
}