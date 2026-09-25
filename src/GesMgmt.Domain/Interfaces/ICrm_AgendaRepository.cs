using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_AgendaRepository
    {
        Task<IQueryable<Crm_Agenda>> Query();
        IQueryable<Crm_Agenda?> GetGestionAgendasDeudor(int nId_Cliente, int nId_Cartera, int nId_PersDeudor, int? nId_PerfilUsuario);
        Task<Crm_Agenda> AddAsync(Crm_Agenda Crm_Agenda);
    }
}