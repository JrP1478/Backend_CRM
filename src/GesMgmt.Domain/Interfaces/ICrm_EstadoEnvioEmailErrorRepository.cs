using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_EstadoEnvioEmailErrorRepository
    {
        Task<IQueryable<Crm_EstadoEnvioEmailError>> Query();
    }
}