using System;
using System.Collections.Generic;
using System.Text;

namespace GesMgmt.Domain.Entities
{
    public class Crm_OpeTipo
    {
        public int nId_OpeTipo { get; set; }
        public string? cNombre_OpeTipo { get; set; }
        public string? cSigla_OpeTipo { get; set; }
        public bool? bEstado { get; set; }
    }
}