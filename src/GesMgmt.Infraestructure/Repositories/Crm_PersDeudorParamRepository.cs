using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersDeudorParamRepository : ICrm_PersDeudorParamRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersDeudorParam> _dbSet;

        public Crm_PersDeudorParamRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersDeudorParam>();
        }

        public async Task<IQueryable<Crm_PersDeudorParam>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_PersDeudorParam?>> GetDeudorParamByIdDeudorAsync(int nId_PersDeudor)
        {
            return _dbSet
                .Where(s => s.nId_PersDeudor == nId_PersDeudor)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_PersDeudorParam?>> GetDeudorParamAsync()
        {
            return _dbSet
                .Include(tg => tg.Crm_Cartera)
                .Where(s => s.Crm_Cartera.bEstado == true)
                .AsNoTracking();
        }
    }
}