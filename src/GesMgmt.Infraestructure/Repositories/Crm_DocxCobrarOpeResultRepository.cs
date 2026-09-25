using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarOpeResultRepository : ICrm_DocxCobrarOpeResultRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrarOpeResult> _dbSet;

        public Crm_DocxCobrarOpeResultRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrarOpeResult>();
        }

        public async Task<IQueryable<Crm_DocxCobrarOpeResult>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_DocxCobrarOpeResult?>> GetReporteCasosByClienteAndCarterasActivoAsync(int nId_Cliente, int nId_Cartera)
        {
            return _dbSet
                .Include(c => c.Crm_Cliente)
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_DocxCobrar)
                .Where(s => s.nId_Cliente == nId_Cliente
                && s.nId_Cartera == nId_Cartera
                && s.bEstado == true)
                .AsNoTracking();
        }

        public async Task<Crm_DocxCobrarOpeResult> GetReporteCasosByIdAsync(int nId_DocxCobrarOpeResult)
        {
            return await _dbSet
                .Include(c => c.Crm_Cliente)
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_DocxCobrar)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_DocxCobrarOpeResult == nId_DocxCobrarOpeResult);
        }

        public async Task<Crm_DocxCobrarOpeResult> AddAsync(Crm_DocxCobrarOpeResult Crm_DocxCobrarOpeResult)
        {
            await _dbSet.AddAsync(Crm_DocxCobrarOpeResult);
            return Crm_DocxCobrarOpeResult;
        }

        public async Task<Crm_DocxCobrarOpeResult> UpdateAsync(Crm_DocxCobrarOpeResult Crm_DocxCobrarOpeResult)
        {
            _dbSet.Update(Crm_DocxCobrarOpeResult);
            return Crm_DocxCobrarOpeResult;
        }
    }
}