using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_asigUsuarioRepository : ICrm_asigUsuarioRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_asigUsuario> _dbSet;

        public Crm_asigUsuarioRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_asigUsuario>();
        }

        public async Task<IQueryable<Crm_asigUsuario>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IEnumerable<Crm_asigUsuario>> GetAsignacionesByIdClienteAndIdUsuarioAsync(int nId_Cliente, int nId_Usuario)
        {
            return await _dbSet
                .Where(x =>
                    x.nid_cliente == nId_Cliente &&
                    x.nid_usuario == nId_Usuario &&
                    x.bestado == true)
                .ToListAsync();
        }
    }
}