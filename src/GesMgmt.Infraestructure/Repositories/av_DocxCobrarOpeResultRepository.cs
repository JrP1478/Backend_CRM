using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class av_DocxCobrarOpeResultRepository : Iav_DocxCobrarOpeResultRepository
    {
        protected readonly AvalDbContext _context;
        protected readonly DbSet<av_DocxCobrarOpeResult> _dbSet;

        public av_DocxCobrarOpeResultRepository(AvalDbContext context)
        {
            _context = context;
            _dbSet = context.Set<av_DocxCobrarOpeResult>();
        }

        public async Task<IQueryable<av_DocxCobrarOpeResult>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<av_DocxCobrarOpeResult?>> GetReporteCasosByClienteAndCarterasActivoAsync(int nId_Cliente, int nId_Cartera)
        {
            return _dbSet
                .Include(c => c.av_Cliente)
                .Include(c => c.av_Cartera)
                .Include(d => d.av_DocxCobrar)
                .Where(s => s.nId_Cliente == nId_Cliente 
                && s.nId_Cartera == nId_Cartera
                && s.bEstado == true)
                .AsNoTracking();
        }

        public async Task<av_DocxCobrarOpeResult> GetReporteCasosByIdAsync(int nId_DocxCobrarOpeResult)
        {
            return await _dbSet
                .Include(c => c.av_Cliente)
                .Include(c => c.av_Cartera)
                .Include(d => d.av_DocxCobrar)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_DocxCobrarOpeResult == nId_DocxCobrarOpeResult);
        }

        public async Task<av_DocxCobrarOpeResult> AddAsync(av_DocxCobrarOpeResult av_DocxCobrarOpeResult)
        {
            await _dbSet.AddAsync(av_DocxCobrarOpeResult);
            return av_DocxCobrarOpeResult;
        }

        public async Task<av_DocxCobrarOpeResult> UpdateAsync(av_DocxCobrarOpeResult av_DocxCobrarOpeResult)
        {
            _dbSet.Update(av_DocxCobrarOpeResult);
            return av_DocxCobrarOpeResult;
        }
    }
}