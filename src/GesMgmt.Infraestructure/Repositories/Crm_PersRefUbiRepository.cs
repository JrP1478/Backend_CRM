using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersRefUbiRepository : ICrm_PersRefUbiRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersRefUbi> _dbSet;

        public Crm_PersRefUbiRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersRefUbi>();
        }

        public async Task<IQueryable<Crm_PersRefUbi>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_PersRefUbi> GetUbicacionesTelefono()
        {
            return _dbSet
                .AsNoTracking()
                .Where(p => p.bEstado == true);
        }

        public IQueryable<Crm_PersRefUbi> GetUbicacionesDireccion()
        {
            return _dbSet
                .AsNoTracking()
                .Where(p => p.bEstado == true);
        }

    }
}