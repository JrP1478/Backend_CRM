using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarOpeRepository : ICrm_DocxCobrarOpeRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrarOpe> _dbSet;

        public Crm_DocxCobrarOpeRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrarOpe>();
        }

        public async Task<IQueryable<Crm_DocxCobrarOpe>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_DocxCobrarOpe?>> GetGestionesCarteraDeudorAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor, int? nId_PerfilUsuario)
        {
            var query = _dbSet
                .Include(dc => dc.Crm_DocxCobrar)
                .Include(tg => tg.Crm_TipoGestion)
                //.Include(u => u.Crm_Cliente)
                .AsNoTracking()
                .AsQueryable();

            if (nId_Cliente > 0)
                query = query.Where(s => s.nId_Cliente == nId_Cliente);

            if (nId_Cartera > 0)
                query = query.Where(s => s.nId_Cartera == nId_Cartera);

            if (nId_PersDeudor > 0)
                query = query.Where(s => s.nId_PersDeudor == nId_PersDeudor);

            if (nId_Cliente != 95 && nId_Cliente != 59)
            {
                if (nId_PerfilUsuario > 0)
                    query = query.Where(s => s.Crm_Usuario.nId_PerfilGest == nId_PerfilUsuario);
            }
            return query;
        }

        public IQueryable<Crm_DocxCobrarOpe?> GetGestionesCarteraDeudorHistoricas(int nId_Cliente, int nId_Cartera, int nId_PersDeudor)
        {
            return _dbSet
                        .Include(dc => dc.Crm_DocxCobrar)
                        .Include(tg => tg.Crm_TipoGestion)
                        //.Include(u => u.Crm_Cliente)
                        .Where(s => s.nId_Cliente == nId_Cliente  &&
                            s.nId_Cartera != nId_Cartera &&
                            s.nId_PersDeudor == nId_PersDeudor &&
                            s.bEstado == true)
                        .AsNoTracking();
        }

        public async Task<Crm_DocxCobrarOpe?> GetDeudorUltimaGestionTipoAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor, int nId_TipoGestion)
        {
            return await _dbSet
                .Include(dc => dc.Crm_DocxCobrar)
                .Include(tg => tg.Crm_TipoGestion)
                .Where(s => s.nId_Cliente == nId_Cliente
                    && s.nId_Cartera == nId_Cartera
                    && s.nId_PersDeudor == nId_PersDeudor
                    && s.bEstado == true
                    && s.nId_TipoGestion == nId_TipoGestion)
                .OrderByDescending(s => s.dDocCobOpe_FecIni)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Crm_DocxCobrarOpe?> GetGestionMejorGestionAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor)
        {
            return await _dbSet
                .Include(dc => dc.Crm_DocxCobrar)
                .Include(tg => tg.Crm_TipoGestion)
                .Include(pg => pg.Crm_OpeCodCliOut)
                .Where(s =>
                    s.nId_Cliente == nId_Cliente &&
                    s.nId_Cartera == nId_Cartera &&
                    s.nId_PersDeudor == nId_PersDeudor &&
                    s.bEstado == true)
                //.OrderByDescending(s => s.Crm_OpeCodCliOut.nPeso)
                .OrderBy(g => g.Crm_OpeCodCliOut.nPeso) // Menor peso primero
                .FirstOrDefaultAsync();
        }

        public IQueryable<Crm_DocxCobrarOpe?> GetGestionListarGestionesAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor)
        {
            return _dbSet
                        .Include(dc => dc.Crm_DocxCobrar)
                        .Include(tg => tg.Crm_TipoGestion)
                        .Include(pg => pg.Crm_OpeCodCliOut)
                        .Where(s =>
                            s.nId_Cliente == nId_Cliente &&
                            s.nId_Cartera == nId_Cartera &&
                            s.nId_PersDeudor == nId_PersDeudor &&
                            s.bEstado == true)
                        .AsNoTracking();
        }

        public async Task<Crm_DocxCobrarOpe> AddAsync(Crm_DocxCobrarOpe Crm_DocxCobrarOpe)
        {
            await _dbSet.AddAsync(Crm_DocxCobrarOpe);
            return Crm_DocxCobrarOpe;
        }

        public async Task<Crm_DocxCobrarOpe> UpdateAsync(Crm_DocxCobrarOpe Crm_DocxCobrarOpe)
        {
            _dbSet.Update(Crm_DocxCobrarOpe);
            return Crm_DocxCobrarOpe;
        }

        public async Task<IQueryable<Crm_DocxCobrarOpe?>> GetGestionesByIdUsuarioToDay(int nId_Cliente, int nId_Usuario)
        {
            DateTime fechaInicio = DateTime.Today;
            DateTime fechaFin = fechaInicio.AddDays(1);

            return _dbSet
            .Include(dc => dc.Crm_OpeCodCliOut)
            .Where(s => s.nId_Cliente == nId_Cliente &&
                s.nId_Usuario == nId_Usuario &&
                s.dDocCobOpe_FecIni >= fechaInicio &&
                s.dDocCobOpe_FecIni < fechaFin &&
                s.bEstado == true)
            .AsNoTracking();
        }
    }
}