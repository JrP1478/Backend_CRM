using Microsoft.EntityFrameworkCore;
using GesMgmt.Domain.Entities;
using GesMgmt.Infraestructure.Configurations;

namespace GesMgmt.Infraestructure.Persistence
{
    public class CrmDbContext: DbContext
    {
        public DbSet<Crm_Agenda> Crm_Agendas { get; set; }
        public DbSet<Crm_asigUsuario> Crm_asigUsuarios { get; set; }
        public DbSet<Crm_BotonCliente> Crm_BotonClientes { get; set; }
        public DbSet<Crm_CabPantallaCob> Crm_CabPantallaCobs { get; set; }
        public DbSet<Crm_CampanaDiscador> Crm_CampanaDiscadors { get; set; }
        public DbSet<Crm_Cartera> Crm_Carteras { get; set; }
        public DbSet<Crm_Cliente> Crm_Clientes { get; set; }
        public DbSet<Crm_ConfigSistema> Crm_ConfigSistemas { get; set; }
        public DbSet<Crm_ContFormTipoParamOpe> Crm_ContFormTipoParamOpes { get; set; }
        public DbSet<Crm_ContFormTipoCrud> Crm_ContFormTipoCruds { get; set; }
        public DbSet<Crm_ContFormularioRptc> Crm_ContFormularioRptcs { get; set; }
        public DbSet<Crm_Contrato> Crm_Contratos { get; set; }
        public DbSet<Crm_DetallePersTelef> Crm_DetallePersTelefs { get; set; }
        public DbSet<Crm_Discador> Crm_Discadors { get; set; }
        public DbSet<Crm_Divisional> Crm_Divisionals { get; set; }
        public DbSet<Crm_DocxCobrarAdicional> Crm_DocxCobrars { get; set; }
        public DbSet<Crm_DocxCobrar> Crm_DocxCobrarAdcionals { get; set; }
        public DbSet<Crm_DocxCobrarCarta> Crm_DocxCobrarCartas { get; set; }
        public DbSet<Crm_DocxCobrarOpe> Crm_DocxCobrarOpes { get; set; }
        public DbSet<Crm_DocxCobrarOpeEst> Crm_DocxCobrarOpeEsts { get; set; }
        public DbSet<Crm_DocxCobrarOpeGes> Crm_DocxCobrarOpeGess { get; set; }
        public DbSet<Crm_DocxCobrarOpeResult> Crm_DocxCobrarOpeResults { get; set; }
        public DbSet<Crm_DocxCobrarParam> Crm_DocxCobrarParams { get; set; }
        public DbSet<Crm_DocxPago> Crm_DocxPagos { get; set; }
        public DbSet<Crm_EstadoAsteriskCrm> Crm_EstadoAsteriskCrms { get; set; }
        public DbSet<Crm_EstadoEnvioEmailGen> Crm_EstadoEnvioEmailGens { get; set; }
        public DbSet<Crm_EstadoEnvioEmailError> Crm_EstadoEnvioEmailErrors { get; set; }
        public DbSet<Crm_FuenteBusTel> Crm_FuenteBusTels { get; set; }
        public DbSet<Crm_Grupo> Crm_Grupos { get; set; }
        public DbSet<Crm_MaeTabla> Crm_MaeTablas { get; set; }
        public DbSet<Crm_Moneda> Crm_Monedas { get; set; }
        public DbSet<Crm_MotivoNoPago> Crm_MotivoNoPagos { get; set; }
        public DbSet<Crm_OficinaCrm> Crm_OficinaCrms { get; set; }
        public DbSet<Crm_Opcion> Crm_Opcions { get; set; }
        public DbSet<Crm_OpeCodCliOutEst> Crm_OpeCodCliOutEsts { get; set; }
        public DbSet<Crm_OpeCodCliOut> Crm_OpeCodCliOuts { get; set; }
        public DbSet<Crm_OpeCodIn> Crm_OpeCodIns { get; set; }
        public DbSet<Crm_OperadorTelefonico> Crm_OperadorTelefonicos { get; set; }
        public DbSet<Crm_OpeTipo> Crm_OpeTipos { get; set; }
        public DbSet<Crm_PasswordHis> Crm_PasswordHiss { get; set; }
        public DbSet<Crm_PersDeudor> Crm_PersDeudors { get; set; }
        public DbSet<Crm_PersDeudorParam> Crm_PersDeudorParams { get; set; }
        public DbSet<Crm_PersDirecc> Crm_PersDireccs { get; set; }
        public DbSet<Crm_PersDeudorGestionHrs> Crm_PersDeudorGestionHrs { get; set; }
        public DbSet<Crm_PersDeudorInfoParamDefCab> Crm_PersDeudorInfoParamDefCabs { get; set; }
        public DbSet<Crm_PersEmail> Crm_PersEmails { get; set; }
        public DbSet<Crm_PersEmailOpe> Crm_PersEmailOpes { get; set; }
        public DbSet<Crm_Perfil> Crm_Perfils { get; set; }
        public DbSet<Crm_PerfilOpcion> Crm_PerfilOpcions { get; set; }
        public DbSet<Crm_PersRefUbi> Crm_PersRefUbis { get; set; }
        public DbSet<Crm_PersTelef> Crm_PersTelefs { get; set; }
        public DbSet<Crm_PersTelefOpeDetalle> Crm_PersTelefOpeDetalles { get; set; }
        public DbSet<Crm_PersTelefOpe> Crm_PersTelefOpes { get; set; }
        public DbSet<Crm_SubZonaGeneral> Crm_SubZonaGenerals { get; set; }
        public DbSet<Crm_TablaCampoGeneral> Crm_TablaCampoGenerals { get; set; }
        public DbSet<Crm_TipoGestion> Crm_TipoGestions { get; set; }
        public DbSet<Crm_UGrupo> Crm_UGrupos { get; set; }
        public DbSet<Crm_Ubigeo> Crm_Ubigeos { get; set; }
        public DbSet<Crm_UsuarioGrupoOpcion> Crm_UsuarioGrupoOpcions { get; set; }
        public DbSet<Crm_Usuario> Crm_Usuarios { get; set; }
        public DbSet<Crm_ZonaCartera> Crm_ZonaCarteras { get; set; }
        public DbSet<Crm_ZonaGeneral> Crm_ZonaGenerals { get; set; }
        public DbSet<RPTC_ReportexCliente> RPTC_ReportexClientes { get; set; }

        public DbSet<ValidationMessage> ValidationMessages { get; set; }

        public CrmDbContext(DbContextOptions<CrmDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Crm_AgendaConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_asigUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_BotonClienteConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_CabPantallaCobConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_CampanaDiscadorConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_CarteraConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ConfigSistemaConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ContFormTipoParamOpeConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ContFormTipoCrudConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ContFormularioRptcConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ContratoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DetallePersTelefConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DiscadorConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DivisionalConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarAdicionalConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarCartaConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarOpeConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarOpeEstConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarOpeGesConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarOpeResultConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxCobrarParamConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_DocxPagoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_EstadoAsteriskCrmConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_EstadoEnvioEmailGenConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_EstadoEnvioEmailErrorConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_GrupoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_FuenteBusTelConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_MaeTablaConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_MonedaConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_MotivoNoPagoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_OficinaCrmConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_OpcionConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_OpeCodCliOutEstConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_OpeCodCliOutConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_OpeCodInConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_OperadorTelefonicoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_OpeTipoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PasswordHisConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersDeudorConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersDeudorParamConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersDireccConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersDeudorGestionHrsConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersDeudorInfoParamDefCabConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersDeudorInfoParamConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersEmailConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersEmailOpeConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PerfilConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PerfilOpcionConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersRefUbiConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersTelefConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersTelefOpeDetalleConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_PersTelefOpeConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_SubZonaGeneralConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_TablaCampoGeneralConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_TipoGestionConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_UbigeoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_UGrupoConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_UsuarioGrupoOpcionConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ZonaCarteraConfiguration());
            modelBuilder.ApplyConfiguration(new Crm_ZonaGeneralConfiguration());
            modelBuilder.ApplyConfiguration(new RPTC_ReportexClienteConfiguration());
            modelBuilder.ApplyConfiguration(new ValidationMessageConfiguration());
        }
    }
}