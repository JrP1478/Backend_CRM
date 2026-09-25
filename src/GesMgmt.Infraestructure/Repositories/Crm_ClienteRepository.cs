using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ClienteRepository : ICrm_ClienteRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Cliente> _dbSet;

        public Crm_ClienteRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Cliente>();
        }

        public async Task<IQueryable<Crm_Cliente>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_Cliente>> ClientesActivosAsync()
        {
            return _dbSet
                .Where(cli => cli.bEstado == true)
                .AsNoTracking();
        }
    }
}