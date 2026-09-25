using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_OpeCodCliOutEstRepository : ICrm_OpeCodCliOutEstRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_OpeCodCliOutEst> _dbSet;

        public Crm_OpeCodCliOutEstRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_OpeCodCliOutEst>();
        }

        public async Task<IQueryable<Crm_OpeCodCliOutEst>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_OpeCodCliOutEst>> EstadoGestionByIdClienteAsync(int nId_Cliente)
        {
            return _dbSet
                    .Where(s => s.nId_Cliente == nId_Cliente && s.bEstado == true)
                    .AsNoTracking();
        }

    }
}