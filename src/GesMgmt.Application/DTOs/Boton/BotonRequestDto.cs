
namespace GesMgmt.Application.DTOs.Boton
{
    public class BotonRequestDto
    {
        public class GetGestionBotonesRequestDto
        {
            public int nId_Cliente { get; set; } //ID_CLIENTE
            public int nId_Contrato { get; set; } //ID_CONTRATO
        }

        public class GetReportarCasosRequestDto
        {
            public int nId_Cliente { get; set; }
            public int nId_Cartera { get; set; }
            public int nId_PersDeudor { get; set; }
        }

        public class GetReportarCasosByIdRequestDto
        {
            public int nId_DocxCobrarOpeResult { get; set; }
            public int nId_Cartera { get; set; }
            public int nId_Cliente { get; set; }
            public int nId_PersDeudor { get; set; }
        }

        public class CreateReportarCasosRequestDto
        {
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

        public class EditReportarCasosRequestDto
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
    }
}