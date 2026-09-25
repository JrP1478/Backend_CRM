using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_DocxCobrarCartaRepository
    {
        Task<IQueryable<Crm_DocxCobrarCarta>> Query();
    }
}