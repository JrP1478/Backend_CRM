using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_PersEmailRepository
    {
        Task<IQueryable<Crm_PersEmail>> Query();
        IQueryable<Crm_PersEmail?> GetEmailsByIdDeudorAsync(int nId_Cliente, int nId_PersDeudor);
        IQueryable<Crm_PersEmail> GetEmailsByIdPersEmail(int nId_PersEmail);
        Task<Crm_PersEmail> AddAsync(Crm_PersEmail Crm_PersEmail);
        Task<Crm_PersEmail> UpdateAsync(Crm_PersEmail Crm_PersEmail);
    }
}