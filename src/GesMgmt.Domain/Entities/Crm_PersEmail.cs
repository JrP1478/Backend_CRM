using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Entities
{
    public class Crm_PersEmail
    {
        public int nId_PersEmail { get; set; }
        public int nId_PersDeudor { get; set; }
        public Crm_PersDeudor Crm_PersDeudor { get; set; }
        public string cPers_Email { get; set; }
        public bool bEstado { get; set; }
        public string? cEmail_Coment { get; set; }
        public string? cEmail_Contacto { get; set; }
        public int nId_Cliente { get; set; }
        public bool bBaseCliente { get; set; }
        public DateTime dFecRegistro { get; set; }
        public int nId_UsuarioAct { get; set; }
        public DateTime dFecActualizacion { get; set; }
        public int? nEmail_Prioridad { get; set; }
        public int? nId_EstadoEnvioEmail { get; set; }
        //public Crm_EstadoEnvioEmailError Crm_EstadoEnvioEmailError { get; set; }
        public string? cEstado { get; set; }
        public DateTime? dFecEstadoEnvio { get; set; }
        public int? nId_EstadoEnvioEmailGen { get; set; }
        //public Crm_EstadoEnvioEmailGen Crm_EstadoEnvioEmailGen { get; set; }
        public DateTime? dFecBaseCliente { get; set; }
        public int? nId_PersEmailOpe { get; set; }
    }
}