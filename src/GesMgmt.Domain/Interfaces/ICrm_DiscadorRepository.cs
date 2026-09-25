using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DiscadorRepository
    {
        Task<IQueryable<Crm_Discador>> Query();
    }
}