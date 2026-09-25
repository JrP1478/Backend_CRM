using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarParamRepository : ICrm_DocxCobrarParamRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrarParam> _dbSet;

        public Crm_DocxCobrarParamRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrarParam>();
        }

        public async Task<IQueryable<Crm_DocxCobrarParam>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_DocxCobrarParam>> GetGestionesParamByIdCarteraAsync(int nId_Cartera)
        {
            return _dbSet.AsNoTracking().Where(p => p.nId_Cartera == nId_Cartera);
        }

        public IQueryable<Crm_DocxCobrarParam> GetGestionesParamAsync(Crm_DocxCobrarParam Crm_DocxCobrarParam)
        {
            var query = _dbSet
                .Include(c => c.Crm_Cartera)
                .Include(dc => dc.Crm_DocxCobrar)
                .AsNoTracking()
                .AsQueryable();

            if (Crm_DocxCobrarParam.nId_Cartera > 0)
                query = query.Where(s => s.nId_Cartera == Crm_DocxCobrarParam.nId_Cartera);

            if (Crm_DocxCobrarParam.nId_DocxCobrar > 0)
                query = query.Where(s => s.nId_DocxCobrar == Crm_DocxCobrarParam.nId_DocxCobrar);

            return query;
        }

    }
}