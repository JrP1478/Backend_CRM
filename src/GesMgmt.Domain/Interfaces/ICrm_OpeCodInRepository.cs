using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_OpeCodInRepository
    {
        Task<IQueryable<Crm_OpeCodIn>> Query();
    }
}