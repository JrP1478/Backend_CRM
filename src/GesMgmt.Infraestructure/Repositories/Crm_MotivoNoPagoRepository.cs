using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_MotivoNoPagoRepository : ICrm_MotivoNoPagoRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_MotivoNoPago> _dbSet;

        public Crm_MotivoNoPagoRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_MotivoNoPago>();
        }

        public async Task<IQueryable<Crm_MotivoNoPago>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_MotivoNoPago>> MotivoNoPagoByIdClienteAsync(int nId_Cliente)
        {
            return _dbSet
                    .Where(s => s.nId_Cliente == nId_Cliente && s.bEstado == true)
                    .AsNoTracking();
        }
    }
}