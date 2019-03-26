using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAESA.Models
{
    public class DetallePagoModel
    {
        public DetallePagoModel() { }

        public string TipoDoc { get; set; }
        public int NumDoc { get; set; }
        public string FechaEmision { get; set; }
        public string Monto { get; set; }
    }
}