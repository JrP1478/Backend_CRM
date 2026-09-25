using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Entities
{
    public class Crm_CampanaDiscador
    {
        public int id { get; set; }
        public int NroCampanaDiscador { get; set; }
        public string cNombreCampana { get; set; }
        public bool? bestado { get; set; }
        public DateTime? FecActualizacion { get; set; }
        public int nId_Cliente { get; set; }
        public Crm_Cliente Crm_Cliente { get; set; }
        public int nId_Discador { get; set; }
        public Crm_Discador Crm_Discador { get; set; }
    }
}