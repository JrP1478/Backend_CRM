using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_CarteraRepository : ICrm_CarteraRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Cartera> _dbSet;

        public Crm_CarteraRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Cartera>();
        }

        public async Task<IQueryable<Crm_Cartera>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_Cartera> GetCarteraByIdClienteIdCarteraAsync(int nId_Cliente, int nId_Cartera)
        {
            return await _dbSet
                .Include(d => d.Crm_Cliente)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_Cliente == nId_Cliente && s.nId_Cartera == nId_Cartera);
        }

        public async Task<IQueryable<Crm_Cartera?>> GetCarteraByClienteCarteraAsync(int nId_Cliente, int nId_Cartera)
        {
            return _dbSet
                .Include(d => d.Crm_Cliente)
                .Where(s => s.nId_Cliente == nId_Cliente && s.nId_Cartera == nId_Cartera)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_Cartera?>> GetCarterasByIdClienteActivoAsync(int nId_Cliente)
        {
            return _dbSet
                .Include(d => d.Crm_Cliente)
                .Where(s => s.nId_Cliente == nId_Cliente && s.bEstado == true)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_Cartera?>> GetCarterasByIdClienteAsync(int nId_Cliente)
        {
            return _dbSet
                .Include(d => d.Crm_Cliente)
                .Where(s => s.nId_Cliente == nId_Cliente)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_Cartera>> GetCarterasParametrosByIdClienteAnnioAsync(int nId_Cliente, int Annio)
        {
            return _dbSet
                .Include(d => d.Crm_Cliente)
                .Where(s => s.nId_Cliente == nId_Cliente
                && s.anio == Annio
                && s.bEstado == true)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_Cartera?>> GetCarterasByIdClienteAndIdCarteraAsync(int nId_Cliente, int nId_Cartera)
        {
            return _dbSet
                .Include(d => d.Crm_Cliente)
                .Where(s => s.nId_Cliente == nId_Cliente && s.nId_Cartera == nId_Cartera)
                .AsNoTracking();
        }
    }
}