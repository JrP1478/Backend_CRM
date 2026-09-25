using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_ConfigSistemaRepository : ICrm_ConfigSistemaRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_ConfigSistema> _dbSet;

        public Crm_ConfigSistemaRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_ConfigSistema>();
        }

        public async Task<IQueryable<Crm_ConfigSistema>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_ConfigSistema> GetConfiguracionSistemaByCodigoTablaAsync(int nCodTabla, string cLlave)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.nCodTabla == nCodTabla && c.cLlave == cLlave);
        }
    }
}