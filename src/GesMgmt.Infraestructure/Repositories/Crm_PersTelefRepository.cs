using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersTelefRepository : ICrm_PersTelefRepository
    {
        protected readonly CrmDbContext  _context;
        protected readonly DbSet<Crm_PersTelef> _dbSet;

        public Crm_PersTelefRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersTelef>();
        }

        public async Task<IQueryable<Crm_PersTelef>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PersTelef> GetTelefonoByIdTelefonoAsync(int nId_PersTelef)
        {
            var query = await _dbSet
                .Include(tel => tel.Crm_PersDeudor)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nId_PersTelef == nId_PersTelef);
            return query;
        }

        public IQueryable<Crm_PersTelef> GetTelefonosAsync(Crm_PersTelef Crm_PersTelef)
        {

            var query = _dbSet
                .Include(tel => tel.Crm_PersDeudor)
                .AsNoTracking()
                .AsQueryable();

            if (Crm_PersTelef.nId_PersDeudor > 0)
                query = query.Where(tel => tel.nId_PersDeudor == Crm_PersTelef.nId_PersDeudor);

            return query;
        }

        public async Task<Crm_PersTelef> GetTelefonoNroTelefonoByIdDeudorAsync(string nTelef_Nro, int nId_PersDeudor)
        {
            var query = await _dbSet
                .Include(tel => tel.Crm_PersDeudor)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nTelef_Nro == nTelef_Nro && s.nId_PersDeudor == nId_PersDeudor);
            return query;
        }

        public async Task<Crm_PersTelef> GetTelefonoNroTelefonoAsync(string nTelef_Nro)
        {
            var query = await _dbSet
                .Include(tel => tel.Crm_PersDeudor)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.nTelef_Nro == nTelef_Nro);
            return query;
        }

        public async Task<Crm_PersTelef> AddAsync(Crm_PersTelef Crm_PersTelef)
        {
            await _dbSet.AddAsync(Crm_PersTelef);
            return Crm_PersTelef;
        }

        public async Task<Crm_PersTelef> UpdateAsync(Crm_PersTelef Crm_PersTelef)
        {
            _dbSet.Update(Crm_PersTelef);
            return Crm_PersTelef;
        }

        public async Task<IQueryable<Crm_PersTelef?>> GetDeudorByTelefonoAsync(string letra, string valor)
        {
            if (letra == "F")
            {
                return _dbSet
                .Where(s => s.nTelef_Nro == valor)
                .AsNoTracking();
            }
            return null;
        }

    }
}