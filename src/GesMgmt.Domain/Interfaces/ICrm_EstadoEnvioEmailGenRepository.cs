using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_EstadoEnvioEmailGenRepository
    {
        Task<IQueryable<Crm_EstadoEnvioEmailGen>> Query();
    }
}