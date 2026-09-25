using GesMgmt.Domain.Interfaces;
using GesMgmt.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace GesMgmt.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Variables
        private readonly CrmDbContext _context;
        private readonly IMemoryCache _cache;
        private IDbContextTransaction? _transaction;

        private ICrm_AgendaRepository? _Crm_Agendas;
        private ICrm_asigUsuarioRepository? _Crm_asigUsuarios;
        private ICrm_BotonClienteRepository? _Crm_BotonClientes;
        private ICrm_CabPantallaCobRepository? _Crm_CabPantallaCobs;
        private ICrm_CampanaDiscadorRepository? _Crm_CampanaDiscadors;
        private ICrm_CarteraRepository? _Crm_Carteras;
        private ICrm_ClienteRepository? _Crm_Clientes;
        private ICrm_ConfigSistemaRepository? _Crm_ConfigSistemas;
        private ICrm_ContFormTipoParamOpeRepository? _Crm_ContFormTipoParamOpes;
        private ICrm_ContFormTipoCrudRepository? _Crm_ContFormTipoCruds;
        private ICrm_ContFormularioRptcRepository? _Crm_ContFormularioRptcs;
        private ICrm_ContratoRepository? _Crm_Contratos;
        private ICrm_DetallePersTelefRepository _Crm_DetallePersTelefs;
        private ICrm_DiscadorRepository? _Crm_Discadors;
        private ICrm_DivisionalRepository _Crm_Divisionals;
        private ICrm_DocxCobrarAdicionalRepository? _Crm_DocxCobrarAdicionals;
        private ICrm_DocxCobrarCartaRepository? _Crm_DocxCobrarCartas;
        private ICrm_DocxCobrarOpeEstRepository? _Crm_DocxCobrarOpeEsts;
        private ICrm_DocxCobrarOpeGesRepository? _Crm_DocxCobrarOpeGess;
        private ICrm_DocxCobrarOpeRepository? _Crm_DocxCobrarOpes;
        private ICrm_DocxCobrarOpeResultRepository? _Crm_DocxCobrarOpeResults;
        private ICrm_DocxCobrarParamRepository? _Crm_DocxCobrarParams;
        private ICrm_DocxCobrarRepository? _Crm_DocxCobrars;
        private ICrm_DocxPagoRepository? _Crm_DocxPagos;
        private ICrm_EstadoAsteriskCrmRepository? _Crm_EstadoAsteriskCrms;
        private ICrm_EstadoEnvioEmailGenRepository? _Crm_EstadoEnvioEmailGens;
        private ICrm_EstadoEnvioEmailErrorRepository? _Crm_EstadoEnvioEmailErrors;
        private ICrm_FuenteBusTelRepository? _Crm_FuenteBusTels;
        private ICrm_GrupoRepository? _Crm_Grupos;
        private ICrm_MaeTablaRepository? _Crm_MaeTablas;
        private ICrm_MonedaRepository? _Crm_Monedas;
        private ICrm_MotivoNoPagoRepository? _Crm_MotivoNoPagos;
        private ICrm_OficinaCrmRepository? _Crm_OficinaCrms;
        private ICrm_OpcionRepository? _Crm_Opcions;
        private ICrm_OpeCodCliOutEstRepository? _Crm_OpeCodCliOutEsts;
        private ICrm_OpeCodCliOutRepository? _Crm_OpeCodCliOuts;
        private ICrm_OpeCodInRepository? _Crm_OpeCodIns;
        private ICrm_OperadorTelefonicoRepository? _Crm_OperadorTelefonicos;
        private ICrm_OpeTipoRepository? _Crm_OpeTipos;
        private ICrm_PasswordHisRepository? _Crm_PasswordHiss;
        private ICrm_PersDeudorGestionHrsRepository? _Crm_PersDeudorGestionHrss;
        private ICrm_PersDeudorInfoParamDefCabRepository? _Crm_PersDeudorInfoParamDefCabs;
        private ICrm_PersDeudorInfoParamRepository? _Crm_PersDeudorInfoParams;
        private ICrm_PersDeudorRepository? _Crm_PersDeudors;
        private ICrm_PersDeudorParamRepository? _Crm_PersDeudorParams;
        private ICrm_PersDireccRepository? _Crm_PersDireccs;
        private ICrm_PerfilRepository? _Crm_Perfils;
        private ICrm_PerfilOpcionRepository? _Crm_PerfilOpcions;
        private ICrm_PersEmailRepository? _Crm_PersEmails;
        private ICrm_PersEmailOpeRepository? _Crm_PersEmailOpes;
        private ICrm_PersRefUbiRepository? _Crm_PersRefUbis;
        private ICrm_PersTelefOpeDetalleRepository? _Crm_PersTelefOpeDetalles;
        private ICrm_PersTelefOpeRepository? _Crm_PersTelefOpes;
        private ICrm_PersTelefRepository? _Crm_PersTelefs;
        private ICrm_SubZonaGeneralRepository? _Crm_SubZonaGenerals;
        private ICrm_TablaCampoGeneralRepository? _Crm_TablaCampoGenerals;
        private ICrm_TipoGestionRepository? _Crm_TipoGestions;
        private ICrm_UbigeoRepository? _Crm_Ubigeos;
        private ICrm_UGrupoRepository? _Crm_UGrupos;
        private ICrm_UsuarioGrupoOpcionRepository _Crm_UsuarioGrupoOpcions;
        private ICrm_UsuarioRepository? _Crm_Usuarios;
        private ICrm_ZonaCarteraRepository? _Crm_ZonaCarteras;
        private ICrm_ZonaGeneralRepository? _Crm_ZonaGenerals;
        private IRPTC_ReportexClienteRepository? _rPTC_ReportexClientes;

        private IValidationMessageRepository? _validationMessages;
        #endregion

        #region Constructor
        public UnitOfWork(CrmDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        #endregion

        #region Properties
        public ICrm_AgendaRepository Crm_Agendas => _Crm_Agendas ??= new Crm_AgendaRepository(_context);
        public ICrm_asigUsuarioRepository Crm_asigUsuarios => _Crm_asigUsuarios ??= new Crm_asigUsuarioRepository(_context);
        public ICrm_BotonClienteRepository Crm_BotonClientes => _Crm_BotonClientes ??= new Crm_BotonClienteRepository(_context);
        public ICrm_CabPantallaCobRepository Crm_CabPantallaCobs => _Crm_CabPantallaCobs ??= new Crm_CabPantallaCobRepository(_context);
        public ICrm_CampanaDiscadorRepository Crm_CampanaDiscadors => _Crm_CampanaDiscadors ??= new Crm_CampanaDiscadorRepository(_context);
        public ICrm_CarteraRepository Crm_Carteras => _Crm_Carteras ??= new Crm_CarteraRepository(_context);
        public ICrm_ClienteRepository Crm_Clientes => _Crm_Clientes ??= new Crm_ClienteRepository(_context);
        public ICrm_ConfigSistemaRepository Crm_ConfigSistemas => _Crm_ConfigSistemas ??= new Crm_ConfigSistemaRepository(_context);
        public ICrm_ContFormTipoParamOpeRepository Crm_ContFormTipoParamOpes => _Crm_ContFormTipoParamOpes ??= new Crm_ContFormTipoParamOpeRepository(_context);
        public ICrm_ContFormTipoCrudRepository Crm_ContFormTipoCruds => _Crm_ContFormTipoCruds ??= new Crm_ContFormTipoCrudRepository(_context);
        public ICrm_ContFormularioRptcRepository Crm_ContFormularioRptcs => _Crm_ContFormularioRptcs ??= new Crm_ContFormularioRptcRepository(_context);
        public ICrm_ContratoRepository Crm_Contratos => _Crm_Contratos ??= new Crm_ContratoRepository(_context);
        public ICrm_DetallePersTelefRepository Crm_DetallePersTelefs => _Crm_DetallePersTelefs ??= new Crm_DetallePersTelefRepository(_context);
        public ICrm_DiscadorRepository Crm_Discadors => _Crm_Discadors ??= new Crm_DiscadorRepository(_context);
        public ICrm_DivisionalRepository Crm_Divisionals => _Crm_Divisionals ??= new Crm_DivisionalRepository(_context);
        public ICrm_DocxCobrarAdicionalRepository Crm_DocxCobrarAdicionals => _Crm_DocxCobrarAdicionals ??= new Crm_DocxCobrarAdicionalRepository(_context);
        public ICrm_DocxCobrarCartaRepository Crm_DocxCobrarCartas => _Crm_DocxCobrarCartas ??= new Crm_DocxCobrarCartaRepository(_context);
        public ICrm_DocxCobrarOpeEstRepository Crm_DocxCobrarOpeEsts => _Crm_DocxCobrarOpeEsts ??= new Crm_DocxCobrarOpeEstRepository(_context);
        public ICrm_DocxCobrarOpeGesRepository Crm_DocxCobrarOpeGess => _Crm_DocxCobrarOpeGess ??= new Crm_DocxCobrarOpeGesRepository(_context);
        public ICrm_DocxCobrarOpeResultRepository Crm_DocxCobrarOpeResults => _Crm_DocxCobrarOpeResults ??= new Crm_DocxCobrarOpeResultRepository(_context);
        public ICrm_DocxCobrarOpeRepository Crm_DocxCobrarOpes => _Crm_DocxCobrarOpes ??= new Crm_DocxCobrarOpeRepository(_context);
        public ICrm_DocxCobrarParamRepository Crm_DocxCobrarParams => _Crm_DocxCobrarParams ??= new Crm_DocxCobrarParamRepository(_context);
        public ICrm_DocxCobrarRepository Crm_DocxCobrars => _Crm_DocxCobrars ??= new Crm_DocxCobrarRepository(_context);
        public ICrm_DocxPagoRepository Crm_DocxPagos => _Crm_DocxPagos ??= new Crm_DocxPagoRepository(_context);
        public ICrm_EstadoAsteriskCrmRepository Crm_EstadoAsteriskCrms => _Crm_EstadoAsteriskCrms ??= new Crm_EstadoAsteriskCrmRepository(_context);
        public ICrm_EstadoEnvioEmailGenRepository Crm_EstadoEnvioEmailGens => _Crm_EstadoEnvioEmailGens ??= new Crm_EstadoEnvioEmailGenRepository(_context);
        public ICrm_EstadoEnvioEmailErrorRepository Crm_EstadoEnvioEmailErrors => _Crm_EstadoEnvioEmailErrors ??= new Crm_EstadoEnvioEmailErrorRepository(_context);
        public ICrm_FuenteBusTelRepository Crm_FuenteBusTels => _Crm_FuenteBusTels ??= new Crm_FuenteBusTelRepository(_context);
        public ICrm_GrupoRepository Crm_Grupos => _Crm_Grupos ??= new Crm_GrupoRepository(_context);
        public ICrm_MaeTablaRepository Crm_MaeTablas => _Crm_MaeTablas ??= new Crm_MaeTablaRepository(_context);
        public ICrm_MonedaRepository Crm_Monedas => _Crm_Monedas ??= new Crm_MonedaRepository(_context);
        public ICrm_MotivoNoPagoRepository Crm_MotivoNoPagos => _Crm_MotivoNoPagos ??= new Crm_MotivoNoPagoRepository(_context);
        public ICrm_OficinaCrmRepository Crm_OficinaCrms => _Crm_OficinaCrms ??= new Crm_OficinaCrmRepository(_context);
        public ICrm_OpcionRepository Crm_Opcions => _Crm_Opcions ??= new Crm_OpcionRepository(_context);
        public ICrm_OpeCodCliOutEstRepository Crm_OpeCodCliOutEsts => _Crm_OpeCodCliOutEsts ??= new Crm_OpeCodCliOutEstRepository(_context);
        public ICrm_OpeCodCliOutRepository Crm_OpeCodCliOuts => _Crm_OpeCodCliOuts ??= new Crm_OpeCodCliOutRepository(_context);
        public ICrm_OpeCodInRepository Crm_OpeCodIns => _Crm_OpeCodIns ??= new Crm_OpeCodInRepository(_context);
        public ICrm_OperadorTelefonicoRepository Crm_OperadorTelefonicos => _Crm_OperadorTelefonicos ??= new Crm_OperadorTelefonicoRepository(_context);
        public ICrm_OpeTipoRepository Crm_OpeTipos => _Crm_OpeTipos ??= new Crm_OpeTipoRepository(_context);
        public ICrm_PasswordHisRepository Crm_PasswordHiss => _Crm_PasswordHiss ??= new Crm_PasswordHisRepository(_context);
        public ICrm_PersDeudorRepository Crm_PersDeudors => _Crm_PersDeudors ??= new Crm_PersDeudorRepository(_context);
        public ICrm_PersDeudorParamRepository Crm_PersDeudorParams => _Crm_PersDeudorParams ??= new Crm_PersDeudorParamRepository(_context);
        public ICrm_PersDeudorGestionHrsRepository Crm_PersDeudorGestionHrss => _Crm_PersDeudorGestionHrss ??= new Crm_PersDeudorGestionHrsRepository(_context);
        public ICrm_PersDeudorInfoParamDefCabRepository Crm_PersDeudorInfoParamDefCabs => _Crm_PersDeudorInfoParamDefCabs ??= new Crm_PersDeudorInfoParamDefCabRepository(_context);
        public ICrm_PersDeudorInfoParamRepository Crm_PersDeudorInfoParams => _Crm_PersDeudorInfoParams ??= new Crm_PersDeudorInfoParamRepository(_context);
        public ICrm_PersDireccRepository Crm_PersDireccs => _Crm_PersDireccs ??= new Crm_PersDireccRepository(_context);
        public ICrm_PersEmailRepository Crm_PersEmails => _Crm_PersEmails ??= new Crm_PersEmailRepository(_context);
        public ICrm_PersEmailOpeRepository Crm_PersEmailOpes => _Crm_PersEmailOpes ??= new Crm_PersEmailOpeRepository(_context);
        public ICrm_PerfilRepository Crm_Perfils => _Crm_Perfils ??= new Crm_PerfilRepository(_context);
        public ICrm_PerfilOpcionRepository Crm_PerfilOpcions => _Crm_PerfilOpcions ??= new Crm_PerfilOpcionRepository(_context);
        public ICrm_PersRefUbiRepository Crm_PersRefUbis => _Crm_PersRefUbis ??= new Crm_PersRefUbiRepository(_context);
        public ICrm_PersTelefOpeDetalleRepository Crm_PersTelefOpeDetalles => _Crm_PersTelefOpeDetalles ??= new Crm_PersTelefOpeDetalleRepository(_context);
        public ICrm_PersTelefOpeRepository Crm_PersTelefOpes => _Crm_PersTelefOpes ??= new Crm_PersTelefOpeRepository(_context);
        public ICrm_PersTelefRepository Crm_PersTelefs => _Crm_PersTelefs ??= new Crm_PersTelefRepository(_context);
        public ICrm_SubZonaGeneralRepository Crm_SubZonaGenerals => _Crm_SubZonaGenerals ??= new Crm_SubZonaGeneralRepository(_context);
        public ICrm_TablaCampoGeneralRepository Crm_TablaCampoGenerals => _Crm_TablaCampoGenerals ??= new Crm_TablaCampoGeneralRepository(_context);
        public ICrm_TipoGestionRepository Crm_TipoGestions => _Crm_TipoGestions ??= new Crm_TipoGestionRepository(_context);
        public ICrm_UbigeoRepository Crm_Ubigeos => _Crm_Ubigeos ??= new Crm_UbigeoRepository(_context);
        public ICrm_UGrupoRepository Crm_UGrupos => _Crm_UGrupos ??= new Crm_UGrupoRepository(_context);
        public ICrm_UsuarioGrupoOpcionRepository Crm_UsuarioGrupoOpcions => _Crm_UsuarioGrupoOpcions ??= new Crm_UsuarioGrupoOpcionRepository(_context);
        public ICrm_UsuarioRepository Crm_Usuarios => _Crm_Usuarios ??= new Crm_UsuarioRepository(_context, _cache);
        public ICrm_ZonaCarteraRepository Crm_ZonaCarteras => _Crm_ZonaCarteras ??= new Crm_ZonaCarteraRepository(_context);
        public ICrm_ZonaGeneralRepository Crm_ZonaGenerals => _Crm_ZonaGenerals ??= new Crm_ZonaGeneralRepository(_context);
        public IRPTC_ReportexClienteRepository RPTC_ReportexClientes => _rPTC_ReportexClientes ??= new RPTC_ReportexClienteRepository(_context);
        public IValidationMessageRepository ValidationMessages => _validationMessages ??= new ValidationMessageRespository(_context);
        #endregion

        #region Methods
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}