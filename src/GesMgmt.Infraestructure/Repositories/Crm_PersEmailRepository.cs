using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersEmailRepository : ICrm_PersEmailRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersEmail> _dbSet;

        public Crm_PersEmailRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersEmail>();
        }

        public async Task<IQueryable<Crm_PersEmail>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_PersEmail?> GetEmailsByIdDeudorAsync(int nId_Cliente, int nId_PersDeudor)
        {
            return _dbSet
                        .Include(dc => dc.Crm_PersDeudor)
                        .Where(s => s.nId_Cliente == nId_Cliente &&
                            s.nId_PersDeudor == nId_PersDeudor)
                        .AsNoTracking();
        }

        public IQueryable<Crm_PersEmail> GetEmailsByIdPersEmail(int nId_PersEmail)
        {
            return _dbSet
                .AsNoTracking()
                .Where(x => x.nId_PersEmail == nId_PersEmail);
        }

        public async Task<Crm_PersEmail> AddAsync(Crm_PersEmail Crm_PersEmail)
        {
            await _dbSet.AddAsync(Crm_PersEmail);
            return Crm_PersEmail;
        }

        public async Task<Crm_PersEmail> UpdateAsync(Crm_PersEmail Crm_PersEmail)
        {
            _dbSet.Update(Crm_PersEmail);
            return Crm_PersEmail;
        }
    }
}