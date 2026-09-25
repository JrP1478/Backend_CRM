using GesMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Application.DTOs.Boton
{
    public class BotonResponseDto
    {
        public class GetGestionBotonesResponseDto
        {
            public int nId_Boton { get; set; }
            public int nId_Cliente { get; set; }
            public int nId_Contrato { get; set; }
            public string nombreBoton { get; set; }
            public string descripcionBoton { get; set; }
            public bool bEstado { get; set; }
            public int nCrea { get; set; }
            public DateTime dFechaCrea { get; set; }
            public int? nModifica { get; set; }
            public DateTime? dFechaModifica { get; set; }
        }

        public class GetReportarCasosResponseDto
        {
            public int Id { get; set; }
            public string Caso { get; set; }
            public string Descripcion { get; set; }
            public string Cartera { get; set; }
            public string Usuario { get; set; }
            public string? Fec_Ingreso { get; set; }
        }

        public class GetReportarCasosByIdResponseDto
        {
            public int nId_DocxCobrarOpeResult { get; set; }
            public int nId_DocxCobrar { get; set; }
            public DateTime? dDocCobOpe_FecIni { get; set; }
            public string? cDocOpeCobOut_Descr { get; set; }
            public int? nId_UsuOpe { get; set; }
            public int nId_PersDeudor { get; set; }
            public int nId_Cartera { get; set; }
            public int nId_Cliente { get; set; }
            public DateTime? dDoc_FecActual { get; set; }
            public string? cDocParam01 { get; set; }
            public string? cDocParam04 { get; set; }
        }

        public class CreateReportarCasosResponseDto
        {
            public int nId_DocxCobrarOpeResult { get; set; }
            public int nId_Cliente { get; set; }
            public int nId_Cartera { get; set; }
            public int nId_DocxCobrar { get; set; }
        }

        public class EditReportarCasosResponseDto
        {
            public int nId_DocxCobrarOpeResult { get; set; }
            public int nId_Cliente { get; set; }
            public int nId_Cartera { get; set; }
            public int nId_DocxCobrar { get; set; }
        }
    }
}