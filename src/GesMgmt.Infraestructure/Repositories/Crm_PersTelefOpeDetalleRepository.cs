using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersTelefOpeDetalleRepository : ICrm_PersTelefOpeDetalleRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersTelefOpeDetalle> _dbSet;

        public Crm_PersTelefOpeDetalleRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersTelefOpeDetalle>();
        }

        public async Task<IQueryable<Crm_PersTelefOpeDetalle>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PersTelefOpeDetalle> AddAsync(Crm_PersTelefOpeDetalle Crm_PersTelefOpeDetalle)
        {
            await _dbSet.AddAsync(Crm_PersTelefOpeDetalle);
            return Crm_PersTelefOpeDetalle;
        }
    }
}