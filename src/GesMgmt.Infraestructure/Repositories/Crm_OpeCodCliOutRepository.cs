using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_OpeCodCliOutRepository : ICrm_OpeCodCliOutRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_OpeCodCliOut> _dbSet;

        public Crm_OpeCodCliOutRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_OpeCodCliOut>();
        }

        public async Task<IQueryable<Crm_OpeCodCliOut>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_OpeCodCliOut> GetTipificacionByIdAsync(int nId_Cliente, int nId_OpeCodCliOut)
        {
            return _dbSet
                .Where(s => s.nId_Cliente == nId_Cliente
                    && s.nId_OpeCodCliOut == nId_OpeCodCliOut)
                .AsNoTracking();
        }

        public async Task<Crm_OpeCodCliOut?> GetTipificacionById2Async(int nId_Cliente, int nId_OpeCodCliOut)
        {
            return await _dbSet
                .Where(s => s.nId_Cliente == nId_Cliente
                    && s.nId_OpeCodCliOut == nId_OpeCodCliOut)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public IQueryable<Crm_OpeCodCliOut> GetGestionPaletaRespuestaAsync(int nId_Cliente, int nId_Contrato, int nNivelPaleta, int? nId_SupOpeCodCliOut, int nId_TipoGestion)
        {
            var query = _dbSet.Where(s =>
                s.nId_Cliente == nId_Cliente &&
                (s.nId_Contrato == null || s.nId_Contrato == nId_Contrato) &&
                s.nNivelPaleta == nNivelPaleta &&
                s.bEstado == true);

            if (nNivelPaleta == 2 && (nId_SupOpeCodCliOut ?? 0) == 0)
            {
                query = query.Where(s => (s.nId_SupOpeCodCliOut ?? 0) == 0);
            }
            else if ((nId_SupOpeCodCliOut ?? 0) > 0)
            {
                query = query.Where(s => s.nId_SupOpeCodCliOut == nId_SupOpeCodCliOut);
            }

            if (nId_TipoGestion != 3)
            {
                query = query.Where(s =>
                    (s.nId_TipoGestion ?? 3) == nId_TipoGestion ||
                    (s.nId_TipoGestion ?? 3) == 3);
            }

            return query.AsNoTracking();
        }

        public async Task<IQueryable<Crm_OpeCodCliOut>> GetTipificacionByIdClienteAsync(int nId_Cliente)
        {
            return _dbSet
                .Where(s => s.nId_Cliente == nId_Cliente)
                .AsNoTracking();
        }
    }
}