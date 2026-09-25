using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarAdicionalRepository : ICrm_DocxCobrarAdicionalRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrarAdicional> _dbSet;

        public Crm_DocxCobrarAdicionalRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrarAdicional>();
        }

        public async Task<IQueryable<Crm_DocxCobrarAdicional>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_DocxCobrarAdicional> GetGestionesAdicionalesAsync(Crm_DocxCobrarAdicional Crm_DocxCobrarAdicional)
        {
            var query = _dbSet
                .Include(c => c.Crm_Cliente)
                .Include(car => car.Crm_Cartera)
                .Include(dc => dc.Crm_DocxCobrar)
                .Include(d => d.Crm_PersDeudor)
                .AsNoTracking()
                .AsQueryable();

            if (Crm_DocxCobrarAdicional.nId_Cliente > 0)
                query = query.Where(s => s.nId_Cliente == Crm_DocxCobrarAdicional.nId_Cliente);

            if (Crm_DocxCobrarAdicional.nId_Cartera > 0)
                query = query.Where(s => s.nId_Cartera == Crm_DocxCobrarAdicional.nId_Cartera);

            if (Crm_DocxCobrarAdicional.nId_PersDeudor > 0)
                query = query.Where(s => s.nId_PersDeudor == Crm_DocxCobrarAdicional.nId_PersDeudor);

            return query;
        }
    }
}