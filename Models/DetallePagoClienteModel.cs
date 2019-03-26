using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAESA.Models
{
    public class DetallePagoClienteModel
    {
        public DetallePagoClienteModel(){}

        public long NroCliente { get; set; }
        public string Empresa { get; set; }
        public string RutCliente { get; set; }
        public string NombreCliente { get; set; }
        public string DvCliente { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Tarifa { get; set; }
        public int TotalDoc { get; set; }
        public string TotalCancelado { get; set; }
        public string TipoDocumento { get; set; }
        public string ResumenPago { get; set; }
        public List<DetallePagoModel> DetallePagos { get; set; }
    }
}