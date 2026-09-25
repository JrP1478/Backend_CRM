using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_BotonClienteRepository : ICrm_BotonClienteRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_BotonCliente> _dbSet;

        public Crm_BotonClienteRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_BotonCliente>();
        }

        public async Task<IQueryable<Crm_BotonCliente>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}