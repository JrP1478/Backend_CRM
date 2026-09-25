using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ContratoRepository : ICrm_ContratoRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Contrato> _dbSet;

        public Crm_ContratoRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Contrato>();
        }

        public async Task<IQueryable<Crm_Contrato>> Query()
        {
            return _dbSet.AsNoTracking();

        }
    }
}