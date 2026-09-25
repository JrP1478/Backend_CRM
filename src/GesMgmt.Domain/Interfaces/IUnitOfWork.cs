
using GesMgmt.Domain.Entities;

namespace GesMgmt.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICrm_AgendaRepository Crm_Agendas { get; }
        ICrm_asigUsuarioRepository Crm_asigUsuarios { get; }
        ICrm_BotonClienteRepository Crm_BotonClientes { get; }
        ICrm_CabPantallaCobRepository Crm_CabPantallaCobs { get; }
        ICrm_CampanaDiscadorRepository Crm_CampanaDiscadors { get; }
        ICrm_CarteraRepository Crm_Carteras { get; }
        ICrm_ClienteRepository Crm_Clientes { get; }
        ICrm_ConfigSistemaRepository Crm_ConfigSistemas { get; }
        ICrm_ContFormTipoCrudRepository Crm_ContFormTipoCruds { get; }
        ICrm_ContFormTipoParamOpeRepository Crm_ContFormTipoParamOpes { get; }
        ICrm_ContFormularioRptcRepository Crm_ContFormularioRptcs { get; }
        ICrm_ContratoRepository Crm_Contratos { get; }
        ICrm_DiscadorRepository Crm_Discadors { get; }
        ICrm_DivisionalRepository Crm_Divisionals { get; }
        ICrm_DetallePersTelefRepository Crm_DetallePersTelefs { get; }
        ICrm_DocxCobrarAdicionalRepository Crm_DocxCobrarAdicionals { get; }
        ICrm_DocxCobrarCartaRepository Crm_DocxCobrarCartas { get; }
        ICrm_DocxCobrarOpeRepository Crm_DocxCobrarOpes { get; }
        ICrm_DocxCobrarOpeEstRepository Crm_DocxCobrarOpeEsts { get; }
        ICrm_DocxCobrarOpeGesRepository Crm_DocxCobrarOpeGess { get; }
        ICrm_DocxCobrarOpeResultRepository Crm_DocxCobrarOpeResults { get; }
        ICrm_DocxCobrarParamRepository Crm_DocxCobrarParams { get; }
        ICrm_DocxCobrarRepository Crm_DocxCobrars { get; }
        ICrm_DocxPagoRepository Crm_DocxPagos { get; }
        ICrm_EstadoAsteriskCrmRepository Crm_EstadoAsteriskCrms { get; }
        ICrm_EstadoEnvioEmailGenRepository Crm_EstadoEnvioEmailGens { get; }
        ICrm_EstadoEnvioEmailErrorRepository Crm_EstadoEnvioEmailErrors { get; }
        ICrm_FuenteBusTelRepository Crm_FuenteBusTels { get; }
        ICrm_GrupoRepository Crm_Grupos { get; }
        ICrm_MaeTablaRepository Crm_MaeTablas { get; }
        ICrm_MonedaRepository Crm_Monedas { get; }
        ICrm_MotivoNoPagoRepository Crm_MotivoNoPagos { get; }
        ICrm_OficinaCrmRepository Crm_OficinaCrms { get; }
        ICrm_OpcionRepository Crm_Opcions { get; }
        ICrm_OpeCodCliOutEstRepository Crm_OpeCodCliOutEsts { get; }
        ICrm_OpeCodCliOutRepository Crm_OpeCodCliOuts { get; }
        ICrm_OpeCodInRepository Crm_OpeCodIns {  get; }
        ICrm_OperadorTelefonicoRepository Crm_OperadorTelefonicos { get;  }
        ICrm_OpeTipoRepository Crm_OpeTipos { get; }
        ICrm_PasswordHisRepository Crm_PasswordHiss { get; }
        ICrm_PersDeudorGestionHrsRepository Crm_PersDeudorGestionHrss { get; }
        ICrm_PersDeudorInfoParamDefCabRepository Crm_PersDeudorInfoParamDefCabs { get; }
        ICrm_PersDeudorInfoParamRepository Crm_PersDeudorInfoParams { get; }
        ICrm_PersDeudorRepository Crm_PersDeudors { get; }
        ICrm_PersDeudorParamRepository Crm_PersDeudorParams { get; }
        ICrm_PersDireccRepository Crm_PersDireccs { get; }
        ICrm_PerfilRepository Crm_Perfils { get; }
        ICrm_PerfilOpcionRepository Crm_PerfilOpcions { get; }
        ICrm_PersEmailRepository Crm_PersEmails { get; }
        ICrm_PersEmailOpeRepository Crm_PersEmailOpes { get; }
        ICrm_PersRefUbiRepository Crm_PersRefUbis { get; }
        ICrm_PersTelefOpeRepository Crm_PersTelefOpes { get; }
        ICrm_PersTelefOpeDetalleRepository Crm_PersTelefOpeDetalles { get; }
        ICrm_PersTelefRepository Crm_PersTelefs { get; }
        ICrm_SubZonaGeneralRepository Crm_SubZonaGenerals { get; }
        ICrm_TablaCampoGeneralRepository Crm_TablaCampoGenerals { get; }
        ICrm_TipoGestionRepository Crm_TipoGestions { get; }
        ICrm_UbigeoRepository Crm_Ubigeos { get; }
        ICrm_UGrupoRepository Crm_UGrupos { get; }
        ICrm_UsuarioGrupoOpcionRepository Crm_UsuarioGrupoOpcions { get; }
        ICrm_UsuarioRepository Crm_Usuarios { get; }
        ICrm_ZonaCarteraRepository Crm_ZonaCarteras { get; }
        ICrm_ZonaGeneralRepository Crm_ZonaGenerals { get; }
        IRPTC_ReportexClienteRepository RPTC_ReportexClientes { get; }

        IValidationMessageRepository ValidationMessages { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}