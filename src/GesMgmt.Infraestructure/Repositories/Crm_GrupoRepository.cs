using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_GrupoRepository : ICrm_GrupoRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Grupo> _dbSet;

        public Crm_GrupoRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Grupo>();
        }

        public async Task<IQueryable<Crm_Grupo>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_Grupo> ByIdAsync(int nId_Grupo)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.nId_Grupo == nId_Grupo).FirstOrDefaultAsync();
        }

        public async Task<Crm_Grupo> ByNombreGrupoAsync(string nombreGrupo)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(p => p.cNombre_Grupo == nombreGrupo).FirstOrDefaultAsync();
        }

        public async Task<IQueryable<Crm_Grupo>> GetGruposByCliente(int nId_Cliente)
        {
            return _dbSet
                .Where(g => g.nid_cliente == nId_Cliente)
                .AsNoTracking();
        }

        public async Task<IQueryable<Crm_Grupo>> GetGruposActivos()
        {
            return _dbSet
                .Where(g => g.bEstado == true)
                .AsNoTracking();
        }

        public async Task<Crm_Grupo> AddAsync(Crm_Grupo Crm_Grupo)
        {
            await _dbSet.AddAsync(Crm_Grupo);
            return Crm_Grupo;
        }

        public async Task<Crm_Grupo> UpdateAsync(Crm_Grupo Crm_Grupo)
        {
            _dbSet.Update(Crm_Grupo);
            return Crm_Grupo;
        }
    }
}