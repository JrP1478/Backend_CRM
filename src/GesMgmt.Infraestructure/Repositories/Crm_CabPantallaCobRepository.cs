using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_CabPantallaCobRepository : ICrm_CabPantallaCobRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_CabPantallaCob> _dbSet;

        public Crm_CabPantallaCobRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_CabPantallaCob>();
        }

        public async Task<IQueryable<Crm_CabPantallaCob>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_CabPantallaCob> GetCabeceraGestionesAsync(Crm_CabPantallaCob Crm_CabPantallaCob)
        {
            return _dbSet
                .AsNoTracking()
                .Where(d => d.nId_Cliente == Crm_CabPantallaCob.nId_Cliente
                       && d.nId_Contrato == Crm_CabPantallaCob.nId_Contrato)
                .OrderBy(s => s.nOrden);
        }
    }
}