using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ContFormularioRptcRepository : ICrm_ContFormularioRptcRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_ContFormularioRptc> _dbSet;

        public Crm_ContFormularioRptcRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_ContFormularioRptc>();
        }

        public async Task<IQueryable<Crm_ContFormularioRptc>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}