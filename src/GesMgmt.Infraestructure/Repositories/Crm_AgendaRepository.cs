using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_AgendaRepository : ICrm_AgendaRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_Agenda> _dbSet;

        public Crm_AgendaRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_Agenda>();
        }

        public async Task<IQueryable<Crm_Agenda>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public IQueryable<Crm_Agenda?> GetGestionAgendasDeudor(int nId_Cliente, int nId_Cartera, int nId_PersDeudor, int? nId_PerfilUsuario)
        {
            return _dbSet
           .AsNoTracking()
           .Where(s =>
                s.nid_Cliente == nId_Cliente &&
                s.nid_Cartera == nId_Cartera &&
                s.nid_PersDeudor == nId_PersDeudor
           );
        }

        public async Task<Crm_Agenda> AddAsync(Crm_Agenda Crm_Agenda)
        {
            await _dbSet.AddAsync(Crm_Agenda);
            return Crm_Agenda;
        }
    }
}