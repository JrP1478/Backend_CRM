using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_ContFormularioRptcRepository
    {
        Task<IQueryable<Crm_ContFormularioRptc>> Query();
    }
}