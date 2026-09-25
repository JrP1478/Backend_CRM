using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DetallePersTelefRepository : ICrm_DetallePersTelefRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DetallePersTelef> _dbSet;

        public Crm_DetallePersTelefRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DetallePersTelef>();
        }

        public async Task<IQueryable<Crm_DetallePersTelef>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_DetallePersTelef> GetDetalleTelefonosAsync(Crm_DetallePersTelef Crm_DetallePersTelef)
        {
            return _dbSet
                .Include(dettel => dettel.Crm_Cliente)
                .Include(dettel => dettel.Crm_PersTelef)
                .AsNoTracking()
                .Where(d => d.nId_Cliente == Crm_DetallePersTelef.nId_Cliente
                       && d.nId_PersTelef == Crm_DetallePersTelef.nId_PersTelef);
        }

        public async Task<Crm_DetallePersTelef> GetDetalleTelefonoSearchAsync(int nId_Cliente, int nId_PersTelef)
        {
            try
            {
                var query = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_Cliente == nId_Cliente && s.nId_PersTelef == nId_PersTelef);
                return query;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Crm_DetallePersTelef> AddAsync(Crm_DetallePersTelef Crm_DetallePersTelef)
        {
            await _dbSet.AddAsync(Crm_DetallePersTelef);
            return Crm_DetallePersTelef;
        }
    }
}