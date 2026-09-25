using Microsoft.EntityFrameworkCore;
using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarRepository : ICrm_DocxCobrarRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrar> _dbSet;

        public Crm_DocxCobrarRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrar>();
        }

        public async Task<IQueryable<Crm_DocxCobrar>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_DocxCobrar>> GetGestionesAsync(Crm_DocxCobrar Crm_DocxCobrar)
        {
            return _dbSet
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_PersDeudor)
                .Include(m => m.Crm_Moneda)
                .Include(u => u.Crm_Usuario)
                .AsNoTracking()
                .Where(d => d.nId_Cliente == Crm_DocxCobrar.nId_Cliente
                       && d.nId_Cartera == Crm_DocxCobrar.nId_Cartera
                       && d.nId_PersDeudor == Crm_DocxCobrar.nId_PersDeudor);
        }

        public async Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarActivosAsync(int nId_Cliente, int nId_PersDeudor)
        {
            return _dbSet
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_PersDeudor)
                .Include(m => m.Crm_Moneda)
                .Include(u => u.Crm_Usuario)
                .AsNoTracking()
                .Where(d => d.nId_Cliente == nId_Cliente
                       && d.nId_PersDeudor == nId_PersDeudor
                       && d.bEstado == 1);
        }

        public async Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByIdClienteAndIdDeudorAsync(int nId_Cliente, int nId_PersDeudor)
        {
            return _dbSet
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_PersDeudor)
                .AsNoTracking()
                .Where(d => d.nId_Cliente == nId_Cliente
                       && d.nId_PersDeudor == nId_PersDeudor);
        }

        public async Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarActivosByIdClienteAsync(int nId_Cliente)
        {
            return _dbSet
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_PersDeudor)
                //.Include(m => m.Crm_Moneda)
                //.Include(u => u.Crm_Usuario)
                .AsNoTracking()
                .Where(d => d.nId_Cliente == nId_Cliente
                       && d.bEstado == 1);
        }

        public async Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByIdClienteAsync(int nId_Cliente)
        {
            return _dbSet
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_PersDeudor)
                //.Include(m => m.Crm_Moneda)
                //.Include(u => u.Crm_Usuario)
                .AsNoTracking()
                .Where(d => d.nId_Cliente == nId_Cliente);
        }

        public async Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByNroDocumentoAsync(string letra, int nId_Cliente, string cDoc_Numero)
        {
            if (letra == "T")
            {
                return _dbSet
                    .AsNoTracking()
                    .Where(d => d.nId_Cliente == nId_Cliente
                           && d.cDoc_Numero == cDoc_Numero);
            }
            if (letra == "C")
            {
                return _dbSet
                    .AsNoTracking()
                    .Where(d => d.nId_Cliente == nId_Cliente
                           && d.cPers_CodCliente == cDoc_Numero);
            }
            return null;
        }

        public async Task<IQueryable<Crm_DocxCobrar>> GetDocumentosxCobrarByClienteAndCarteraAsync(int nId_Cliente, int nId_Cartera)
        {
            return _dbSet
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_PersDeudor)
                .AsNoTracking()
                .Where(d => d.nId_Cliente == nId_Cliente
                       && d.nId_Cartera == nId_Cartera);
        }

        public async Task<Crm_DocxCobrar> GetDocxCobByClienteAndDeudorActivoAsync(int nId_Cliente, int nId_Cartera, int nId_PersDeudor)
        {
            return await _dbSet
                .Include(cli => cli.Crm_Cliente)
                .Include(c => c.Crm_Cartera)
                .Include(d => d.Crm_PersDeudor)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_Cliente == nId_Cliente
                                    && s.nId_Cartera == nId_Cartera
                                    && s.nId_PersDeudor == nId_PersDeudor
                                    && s.bEstado == 1);
        }

    }
}