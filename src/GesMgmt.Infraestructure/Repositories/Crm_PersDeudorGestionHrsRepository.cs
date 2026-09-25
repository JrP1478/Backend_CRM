using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersDeudorGestionHrsRepository : ICrm_PersDeudorGestionHrsRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersDeudorGestionHrs> _dbSet;

        public Crm_PersDeudorGestionHrsRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersDeudorGestionHrs>();
        }

        public async Task<IQueryable<Crm_PersDeudorGestionHrs>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_PersDeudorGestionHrs> GetHorarioGestionTelefono()
        {
            return _dbSet
                .AsNoTracking()
                .Where(p => p.bEstado == true);
        }
    }
}