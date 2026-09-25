using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_TablaCampoGeneralRepository : ICrm_TablaCampoGeneralRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_TablaCampoGeneral> _dbSet;

        public Crm_TablaCampoGeneralRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_TablaCampoGeneral>();
        }

        public async Task<IQueryable<Crm_TablaCampoGeneral>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_TablaCampoGeneral>> GetCabeceraGestionesAdicionalAsync(Crm_TablaCampoGeneral Crm_TablaCampoGeneral)
        {
            var query = _dbSet
                .AsNoTracking()
                .AsQueryable();

            if (Crm_TablaCampoGeneral.nId_Cliente > 0)
                query = query.Where(s => s.nId_Cliente == Crm_TablaCampoGeneral.nId_Cliente);

            if (Crm_TablaCampoGeneral.pantalla > 0)
                query = query.Where(s => s.pantalla == Crm_TablaCampoGeneral.pantalla);

            return query;
        }

    }
}