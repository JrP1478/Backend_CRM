using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersDeudorInfoParamRepository : ICrm_PersDeudorInfoParamRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersDeudorInfoParam> _dbSet;

        public Crm_PersDeudorInfoParamRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersDeudorInfoParam>();
        }

        public async Task<IQueryable<Crm_PersDeudorInfoParam>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PersDeudorInfoParam> GetGestionInformacionDeudorParamAsync(int nId_PersDeudor)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_PersDeudor == nId_PersDeudor);
        }
    }
}