using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_DocxCobrarOpeGesRepository : ICrm_DocxCobrarOpeGesRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_DocxCobrarOpeGes> _dbSet;

        public Crm_DocxCobrarOpeGesRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_DocxCobrarOpeGes>();
        }

        public async Task<IQueryable<Crm_DocxCobrarOpeGes>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_DocxCobrarOpeGes> AddAsync(Crm_DocxCobrarOpeGes Crm_DocxCobrarOpeGes)
        {
            await _dbSet.AddAsync(Crm_DocxCobrarOpeGes);
            return Crm_DocxCobrarOpeGes;
        }

        public async Task<IEnumerable<Crm_DocxCobrarOpeGes>> AddRangeAsync(IEnumerable<Crm_DocxCobrarOpeGes> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public async Task<Crm_DocxCobrarOpeGes> UpdateAsync(Crm_DocxCobrarOpeGes Crm_DocxCobrarOpeGes)
        {
            _dbSet.Update(Crm_DocxCobrarOpeGes);
            return Crm_DocxCobrarOpeGes;
        }
    }
}