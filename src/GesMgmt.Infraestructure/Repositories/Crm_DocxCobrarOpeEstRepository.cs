using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarOpeEstRepository : ICrm_DocxCobrarOpeEstRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrarOpeEst> _dbSet;

        public Crm_DocxCobrarOpeEstRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrarOpeEst>();
        }

        public async Task<IQueryable<Crm_DocxCobrarOpeEst>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_DocxCobrarOpeEst> GetGestionesEstadoCarteraDeudor(int nId_Cliente, int nId_Cartera, int nId_PersDeudor)
        {
            var query = _dbSet
                .Include(tg => tg.Crm_TipoGestion)
                .Include(tg => tg.Crm_Usuario)
                .Include(tg => tg.Crm_OpeCodCliOutEst)
                .AsNoTracking()
                .AsQueryable();

            if (nId_Cliente > 0)
                query = query.Where(s => s.nId_Cliente == nId_Cliente);

            if (nId_Cartera > 0)
                query = query.Where(s => s.nId_Cartera == nId_Cartera);

            if (nId_PersDeudor > 0)
                query = query.Where(s => s.nId_PersDeudor == nId_PersDeudor);

            return query;
        }

        public IQueryable<Crm_DocxCobrarOpeEst> GetGestionesEstadoCarteraDeudorHistoricas(int nId_Cliente, int nId_Cartera, int nId_PersDeudor)
        {
            return _dbSet
                        .Include(tg => tg.Crm_TipoGestion)
                        .Include(tg => tg.Crm_Usuario)
                        .Include(tg => tg.Crm_OpeCodCliOutEst)
                        .Where(s => s.nId_Cliente == nId_Cliente &&
                            s.nId_Cartera != nId_Cartera &&
                            s.nId_PersDeudor == nId_PersDeudor &&
                            s.bEstado == true)
                        .AsNoTracking();
        }
    }
}