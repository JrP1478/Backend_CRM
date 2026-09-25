using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_PersDeudorInfoParamDefCabRepository : ICrm_PersDeudorInfoParamDefCabRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_PersDeudorInfoParamDefCab> _dbSet;

        public Crm_PersDeudorInfoParamDefCabRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_PersDeudorInfoParamDefCab>();
        }

        public async Task<IQueryable<Crm_PersDeudorInfoParamDefCab>> Query()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task<Crm_PersDeudorInfoParamDefCab> GetPersDeudorInfoParamDefCabAsync(bool tipoCabecera)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.bTipo_Cabecera.Equals(tipoCabecera));
        }
    }
}