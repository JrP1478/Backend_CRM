using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_asigUsuarioRepository
    {
        Task<IQueryable<Crm_asigUsuario>> Query();
        Task<IEnumerable<Crm_asigUsuario>> GetAsignacionesByIdClienteAndIdUsuarioAsync(int nId_Cliente, int nId_Usuario);
    }
}