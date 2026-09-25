using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersEmailOpeRepository
    {
        Task<IQueryable<Crm_PersEmailOpe>> Query();
    }
}