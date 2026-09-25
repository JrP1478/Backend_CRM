using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ContFormTipoParamOpeRepository : ICrm_ContFormTipoParamOpeRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_ContFormTipoParamOpe> _dbSet;

        public Crm_ContFormTipoParamOpeRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_ContFormTipoParamOpe>();
        }

        public async Task<IQueryable<Crm_ContFormTipoParamOpe>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}