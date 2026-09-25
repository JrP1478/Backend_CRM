using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ZonaCarteraRepository : ICrm_ZonaCarteraRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_ZonaCartera> _dbSet;

        public Crm_ZonaCarteraRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_ZonaCartera>();
        }

        public async Task<IQueryable<Crm_ZonaCartera>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_ZonaCartera> GetZonaCarteraByIdClienteAsync(int nId_Cliente)
        {
            return await _dbSet
                .Include(d => d.Crm_Divisional)
                .Include(d => d.Crm_OficinaCrm)
                .Include(d => d.Crm_Usuario)
                .Include(d => d.Crm_SubZonaGeneral)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nid_cliente == nId_Cliente);
        }

        public async Task<IQueryable<Crm_ZonaCartera?>> GetZonasCarterasByIdClienteAsync(int nId_Cliente)
        {
            return _dbSet
                .Include(d => d.Crm_Divisional)
                .Include(d => d.Crm_OficinaCrm)
                .Include(d => d.Crm_Usuario)
                .Include(d => d.Crm_SubZonaGeneral)
                .Where(s => s.nid_cliente == nId_Cliente)
                .AsNoTracking();
        }

    }
}