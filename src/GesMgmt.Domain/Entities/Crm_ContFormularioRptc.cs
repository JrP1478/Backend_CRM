
namespace GesMgmt.Domain.Entities
{
    public class Crm_ContFormularioRptc
    {
        public int nId_ContFormRptc { get; set; }
        public int nId_Contrato { get; set; }
        public Crm_Contrato Crm_Contrato { get; set; }
        public int nTipoFormCrud { get; set; }
        public Crm_ContFormTipoCrud Crm_ContFormTipoCrud { get; set; }
        public string? cScriptStoreParamList { get; set; }
        public int nTipoFormParamOpe { get; set; }
        public Crm_ContFormTipoParamOpe Crm_ContFormTipoParamOpe { get; set; }
        public int nId_ReporteFormParam { get; set; }
        public RPTC_ReportexCliente RPTC_ReportexCliente { get; set; }
        public string? cScriptStoreParamEdit { get; set; }
        public bool bEstado { get; set; }
        public string? cNombreFormLink { get; set; }
    }
}