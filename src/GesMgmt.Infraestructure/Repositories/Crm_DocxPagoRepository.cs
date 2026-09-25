using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxPagoRepository : ICrm_DocxPagoRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxPago> _dbSet;

        public Crm_DocxPagoRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxPago>();
        }

        public async Task<IQueryable<Crm_DocxPago>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_DocxPago?> GetPagosByIdDeudorAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor)
        {
            return _dbSet
                        .Where(s => s.nId_Cliente == nId_Cliente &&
                            s.nId_Cartera == nId_Cartera &&
                            s.nId_PersDeudor == nId_PersDeudor)
                        .AsNoTracking();
        }
    }
}