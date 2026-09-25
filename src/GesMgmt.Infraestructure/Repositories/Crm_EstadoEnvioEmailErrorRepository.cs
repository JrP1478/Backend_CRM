using GesMgmt.Domain.Entities;
using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Infraestructure.Repositories
{
    public class Crm_EstadoEnvioEmailErrorRepository : ICrm_EstadoEnvioEmailErrorRepository
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<Crm_EstadoEnvioEmailError> _dbSet;

        public Crm_EstadoEnvioEmailErrorRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Crm_EstadoEnvioEmailError>();
        }

        public async Task<IQueryable<Crm_EstadoEnvioEmailError>> Query()
        {
            return _dbSet.AsNoTracking();
        }
    }
}