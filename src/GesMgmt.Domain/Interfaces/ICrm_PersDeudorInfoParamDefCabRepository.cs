using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersDeudorInfoParamDefCabRepository
    {
        Task<IQueryable<Crm_PersDeudorInfoParamDefCab>> Query();
        Task<Crm_PersDeudorInfoParamDefCab> GetPersDeudorInfoParamDefCabAsync(bool tipoCabecera);
    }
}