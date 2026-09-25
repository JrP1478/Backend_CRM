using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface ICrm_OpeCodCliOutRepository
    {
        Task<IQueryable<Crm_OpeCodCliOut>> Query();
        IQueryable<Crm_OpeCodCliOut> GetTipificacionByIdAsync(int nId_Cliente, int nId_OpeCodCliOut);
        Task<Crm_OpeCodCliOut?> GetTipificacionById2Async(int nId_Cliente, int nId_OpeCodCliOut);
        IQueryable<Crm_OpeCodCliOut> GetGestionPaletaRespuestaAsync(int nId_Cliente, int nId_Contrato, int nNivelPaleta, int? nId_SupOpeCodCliOut, int nId_TipoGestion);
        Task<IQueryable<Crm_OpeCodCliOut>> GetTipificacionByIdClienteAsync(int nId_Cliente);
    }
}