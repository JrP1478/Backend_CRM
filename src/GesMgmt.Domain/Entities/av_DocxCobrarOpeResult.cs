
namespace GesMgmt.Domain.Entities
{
    public class av_DocxCobrarOpeResult
    {
        public int nId_DocxCobrarOpeResult { get; set; }
        public int nId_DocxCobrar { get; set; }
        public av_DocxCobrar av_DocxCobrar { get; set; }
        public DateTime? dDocCobOpe_FecIni { get; set; }
        public int? nId_OpeCodOut { get; set; }
        public bool? bEstado { get; set; }
        public int? nId_UsuOpe { get; set; }
        public string? cDocOpeCobOut_Descr { get; set; }
        public int nId_Cliente { get; set; }
        public av_Cliente av_Cliente { get; set; }
        public int nId_Cartera { get; set; }
        public av_Cartera av_Cartera { get; set; }
        public int nId_PersDeudor { get; set; }
        public DateTime? dDoc_FecIngresoGes { get; set; }
        public string? cDocParam01 { get; set; }
        public string? cDocParam02 { get; set; }
        public string? cDocParam03 { get; set; }
        public string? cDocParam04 { get; set; }
        public string? cDocParam05 { get; set; }
        public string? cDocParam06 { get; set; }
        public string? cDocParam07 { get; set; }
        public string? cDocParam08 { get; set; }
        public string? cDocParam09 { get; set; }
        public string? cDocParam10 { get; set; }
        public string? cDocParam11 { get; set; }
        public string? cDocParam12 { get; set; }
        public string? cDocParam13 { get; set; }
        public string? cDocParam14 { get; set; }
        public string? cDocParam15 { get; set; }
        public string? cDocParam16 { get; set; }
        public string? cDocParam17 { get; set; }
        public string? cDocParam18 { get; set; }
        public string? cDocParam19 { get; set; }
        public string? cDocParam20 { get; set; }
        public string? cDocParam21 { get; set; }
        public string? cDocParam22 { get; set; }
        public string? cDocParam23 { get; set; }
        public string? cDocParam24 { get; set; }
        public string? cDocParam25 { get; set; }
        public string? cDocParam26 { get; set; }
        public string? cDocParam27 { get; set; }
        public string? cDocParam28 { get; set; }
        public string? cDocParam29 { get; set; }
        public string? cDocParam30 { get; set; }
        public int? nId_DocumentoPago { get; set; }
        public DateTime? dDoc_FecActual { get; set; }
    }
}