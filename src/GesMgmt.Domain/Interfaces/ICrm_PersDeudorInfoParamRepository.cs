using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersDeudorInfoParamRepository
    {
        Task<IQueryable<Crm_PersDeudorInfoParam>> Query();
        Task<Crm_PersDeudorInfoParam> GetGestionInformacionDeudorParamAsync(int nId_PersDeudor);
    }
}