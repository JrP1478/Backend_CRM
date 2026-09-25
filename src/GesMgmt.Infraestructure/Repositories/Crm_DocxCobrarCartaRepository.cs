using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarCartaRepository : ICrm_DocxCobrarCartaRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrarCarta> _dbSet;

        public Crm_DocxCobrarCartaRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrarCarta>();
        }

        public async Task<IQueryable<Crm_DocxCobrarCarta>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}