using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PasswordHisRepository : ICrm_PasswordHisRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PasswordHis> _dbSet;

        public Crm_PasswordHisRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PasswordHis>();
        }

        public async Task<IQueryable<Crm_PasswordHis>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PasswordHis> ByIdAsync(int nId_PasswordHis)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_PasswordHis == nId_PasswordHis).FirstOrDefaultAsync();
        }

        public async Task<Crm_PasswordHis> ByClavePorFechaAsync(int nId_Usuario, string cUsr_Pass, DateTime dFecRegistro)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_Usuario == nId_Usuario && p.cUsr_Pass == cUsr_Pass && p.dFecRegistro > dFecRegistro).FirstOrDefaultAsync();
        }

        public async Task<Crm_PasswordHis> AddAsync(Crm_PasswordHis Crm_PasswordHis)
        {
            await _dbSet.AddAsync(Crm_PasswordHis);
            return Crm_PasswordHis;
        }

        public async Task<Crm_PasswordHis> UpdateAsync(Crm_PasswordHis Crm_PasswordHis)
        {
            _dbSet.Update(Crm_PasswordHis);
            return Crm_PasswordHis;
        }
    }
}