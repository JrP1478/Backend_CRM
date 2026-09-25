using GesMgmt.Domain.Entities;

namespace GesMgmt.Application.DTOs.Telefono
{
    public class TelefonoResponseDto
    {
        public class GetTelefonosResponseDto
        {
            public int nId_PersTelef { get; set; }
            public int? prioridad { get; set; }
            public string? nroTelefono { get; set; }
            public string? horario { get; set; }
            public string? referenciaUbicacion { get; set; }
            public string? estado { get; set; }
            public string? fechaEstado { get; set; }
            public string? fechaBase { get; set; }
            public string? contactados { get; set; }
            public int? noContactados { get; set; }
            public int? cantidadIvr { get; set; }
            public string? fuente { get; set; }
            public string? ordenSearch { get; set; }
        }

        public class GetTelefonoResultados
        {
            public int nId_PersTelefOpe { get; set; }
            public string? cNombre_PersTelefOpe { get; set; }
            public string? cSigla_PersTelefOpe { get; set; }
            public bool? bEstado { get; set; }
        }

        public class GetTelefonoOperadores
        {
            public int nId_OperadorTelefonico { get; set; }
            public string? cNombreOperadorTelef { get; set; }
            public string? cAbrevOperadorTelef { get; set; }
            public bool? bEstado { get; set; }
        }

        public class GetTelefonoUbicaciones
        {
            public int nId_PersRefUbi { get; set; }
            public string? cNombre_PersRefUbi { get; set; }
            public string? cSigla_PersRefUbi { get; set; }
            public bool? bEstado { get; set; }
            public int? nGestionMovil { get; set; }
        }

        public class GetTelefonoHorarioGestion
        {
            public int nId_PersDeudorGestionHrs { get; set; }
            public string? cNombren_PersDeudorGestionHrs { get; set; }
            public string? cSigla_PersDeudorGestionHrs { get; set; }
            public bool? bEstado { get; set; }
            public int? nHr_ini { get; set; }
            public int? nHr_fin { get; set; }
        }

        public class GetTelefonoFuenteBusqueda
        {
            public int nId_Fuente { get; set; }
            public string? cDescripcion { get; set; }
            public int? nId_Cliente_Ref { get; set; }
            public string? nId_Referencia { get; set; }
            public string? cNombre_Referencia { get; set; }
        }

        public class GetTelefonoAsync
        {
            public int nId_PersTelef { get; set; }
            public int? nId_PersDeudor { get; set; }
            public Crm_PersDeudor? Crm_PersDeudor { get; set; }
            public string? nTelef_Pre { get; set; }
            public string? nTelef_Nro { get; set; }
            public string? nTelef_Anexo { get; set; }
            public int? nId_PersRefUbi { get; set; } //en crm Ubicación*
            //public Crm_PersRefUbi? Crm_PersRefUbi { get; set; }
            public string? cTelef_Coment { get; set; }
            public bool? bEstado { get; set; }
            public int? nId_PersDirecc { get; set; }
            public int? nTelef_Prioridad { get; set; }
            public int? nId_PersTelefOpe { get; set; } //en crm Resultado*
            //public Crm_PersTelefOpe Crm_PersTelefOpe { get; set; }
            public int? nId_PersDeudorGestionHrs { get; set; } // en crm Horario de Gestión
            //public Crm_PersDeudorGestionHrs? Crm_PersDeudorGestionHrs { get; set; }
            public string? dFecUlt_PerstelefOpe { get; set; }
            public string? dFecCarga_PersTelef { get; set; }
            public string? cDireccionTEMPORAL { get; set; }
            public int? ncontactados { get; set; }
            public string? baseTelef { get; set; }
            public string? cbus { get; set; }
            public int? nId_Fuente { get; set; } //en crm Fuente Búsqueda
            public int? nreferencia { get; set; }
            public int? nid_usuarioupd { get; set; }
            public int? nId_OperadorTelefonico { get; set; } //en crm Operador Telefónico*
            public int? nId_EstadoAstkProv { get; set; }
            public string? dFec_EstadoAstkProv { get; set; }
            public int? nId_TipoTelefono { get; set; }
            public int? nNoContactados { get; set; }
            public int? nCant_Ivr { get; set; }
            public int? nOrden_Act { get; set; }
            public bool? bReclamo { get; set; }
            public string? c_osiptel { get; set; }
            public string? c_modalidad_osiptel { get; set; }
            public string? c_operadora_osiptel { get; set; }
            public string? f_estado_osiptel { get; set; }
            public string? Nombre { get; set; }
            public string? Contacto { get; set; }
            public string? Parentesco { get; set; }
        }

        public class CreateTelefonoResponseDto
        {
            public int nId_PersTelef { get; set; }
            public int? nId_PersDeudor { get; set; }
            public string? nTelef_Nro { get; set; }
        }

        public class EditTelefonoResponseDto
        {
            public int nId_PersTelef { get; set; }
            public int? nId_PersDeudor { get; set; }
            public string? nTelef_Nro { get; set; }
        }
    }
}