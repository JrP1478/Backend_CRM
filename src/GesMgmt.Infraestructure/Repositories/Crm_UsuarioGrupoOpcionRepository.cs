using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_UsuarioGrupoOpcionRepository : ICrm_UsuarioGrupoOpcionRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_UsuarioGrupoOpcion> _dbSet;

        public Crm_UsuarioGrupoOpcionRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_UsuarioGrupoOpcion>();
        }

        public async Task<IQueryable<Crm_UsuarioGrupoOpcion>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<IQueryable<Crm_UsuarioGrupoOpcion>> ByIdUsuarioIdGrupoAsync(int nId_Usuario, int nId_Grupo)
        {
            return _dbSet.AsNoTracking().Where(p => p.nId_Usuario == nId_Usuario && p.nId_Grupo == nId_Grupo);
        }

        public async Task<Crm_UsuarioGrupoOpcion> ByIdAsync(int nId_UsuarioGrupoOpcion)
        {
            return await _dbSet
                .AsNoTracking().Where(p => p.nId_UsuarioGrupoOpcion == nId_UsuarioGrupoOpcion).FirstOrDefaultAsync();
        }

        public async Task<Crm_UsuarioGrupoOpcion> AddAsync(Crm_UsuarioGrupoOpcion Crm_UsuarioGrupoOpcion)
        {
            await _dbSet.AddAsync(Crm_UsuarioGrupoOpcion);
            return Crm_UsuarioGrupoOpcion;
        }

        public async Task<Crm_UsuarioGrupoOpcion> UpdateAsync(Crm_UsuarioGrupoOpcion Crm_UsuarioGrupoOpcion)
        {
            _dbSet.Update(Crm_UsuarioGrupoOpcion);
            return Crm_UsuarioGrupoOpcion;
        }

    }
}