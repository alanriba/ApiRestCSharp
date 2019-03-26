using NiiPrinterCLib;
using SAESA.AutoAtencion.Dao;
using SAESA.AutoAtencion.Framework.Configuracion;
using SAESA.AutoAtencion.Servicios.Pago;
using SAESA.AutoAtencion.Servicios.Services;
using SAESA.Models;
using SAESA.Models.Response;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace SAESA.Controllers
{
    /// <summary>
    /// Class controller
    /// </summary>
    [RoutePrefix("api/print")]
    public class PrintController : ApiController
    {
        private PipeService pipe = null;
        [HttpPost]
        [AcceptVerbs("POST")]
        [ActionName("printDocumento")]
        [Route("document/{tipo}", Order = 1, Name = "printDocumento")]
        public IHttpActionResult PrintDocument(string tipo, [FromBody] string url)
        {
            pipe = new PipeService();
            string respuesta = pipe.ComponentPipe(url);

            return Ok(respuesta);
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [ActionName("printCertificado")]
        [Route("certificado", Order = 5, Name = "printCertificado")]
        public IHttpActionResult PrintCertificado([FromBody] CertificadoModel certificado)
        {
            pipe = new PipeService();
            var oConfig = Configuracion.Instancia();
            string respuesta = string.Empty;

            if (certificado != null)
            {
                DateTime fechaInicio = new DateTime();
                DateTime fechaTermino = new DateTime();

                fechaInicio = Convert.ToDateTime(certificado.FechaInicio);
                fechaTermino = Convert.ToDateTime(certificado.FechaTermino);

                string urlCertificado = oConfig.IpEndPoint + ':' 
                    + oConfig.PortEndPoint + 
                    oConfig.UrlRelativa + "SaesaCertificados/" + 
                    certificado.idEmp + "/" + 
                    certificado.NumCli + "/" + 
                    fechaInicio + "/" + 
                    fechaTermino + '/' + 
                    certificado.TipCert + "WS";
                respuesta = pipe.ComponentPipeDownLoadFile(urlCertificado);
            }
            return Ok(respuesta);
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [ActionName("printTbk")]
        [Route("printTbk", Order = 2, Name = "printTbk")]
        public IHttpActionResult PrintDctoTbk([FromBody] DetallePagoClienteModel detalle)
        {
            string msg = string.Empty;

            int ret;
            long jid;
            string aux;
            string printerName = "NII ExD NP-K205";

            pipe = new PipeService();
            
            var respuesta = pipe.ComponentPipePrintVoutcher(printerName, detalle.ResumenPago);
            Log.Write("Usabilidad", msg, Evento.Response, ServicioPago.Comandos.PagoTbk);
            return Ok(new ApiResponse() { Code = 0, Message = "OK".ToString() });
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [ActionName("printDctoPago")]
        [Route("cupon", Order = 3, Name = "printDctoPago")]
        public IHttpActionResult PrintDctoPago([FromBody] DetallePagoClienteModel detalle)
        {
            pipe = new PipeService();

            DetallePagoClienteDao cuponPago = new DetallePagoClienteDao();

            cuponPago.NroCliente = detalle.NroCliente;
            cuponPago.Empresa = detalle.Empresa;
            cuponPago.NombreCliente = detalle.NombreCliente;
            cuponPago.RutCliente = detalle.RutCliente;
            cuponPago.DvCliente = detalle.DvCliente;
            cuponPago.Direccion = detalle.Direccion;
            cuponPago.Comuna = detalle.Comuna;
            cuponPago.Tarifa = detalle.Tarifa;
            cuponPago.TipoDocumento = Convert.ToInt32(detalle.TipoDocumento);
            cuponPago.TotalDoc = detalle.TotalDoc;
            cuponPago.TotalCancelado = detalle.TotalCancelado;

            if (detalle.DetallePagos.Count > 0)
            {
                var i = 0;
                cuponPago.DetallePagos = new List<DetallePagoDao>();
                foreach (var d in detalle.DetallePagos)
                {
                    var det = new DetallePagoDao();
                    det.FechaEmision = d.FechaEmision;
                    det.Monto = d.Monto;
                    det.NumDoc = d.NumDoc;
                    det.TipoDoc = d.TipoDoc;
                    cuponPago.DetallePagos.Add(det);

                    string respuesta = pipe.ComponentPipe(cuponPago);
                    i++;
                }
            }


            // string respuesta = pipe.ComponentPipe(cuponPago);

            return Ok();
        }

        [HttpPost]
        [AcceptVerbs("GET")]
        [ActionName("printDctoConsumo")]
        [Route("historial/{idEmp}/{idServ}", Order = 4, Name = "printDctoConsumo")]
        public IHttpActionResult PrintDctoConsumo(int idEmp, long idServ, [FromBody] DetallePagoClienteModel detalle)
        {
            string arreglo = string.Empty;
            string respuesta = string.Empty;
            string detalleCliente = string.Empty;

            var pagging = new PaginParameterModel() { _pageSize = 30 };
            IHttpActionResult httpResp = new CuponController().HistorialConsumo(idEmp, idServ, pagging);

            var items = ((OkNegotiatedContentResult<ResponseClient<CuponObj>>)httpResp).Content;

            if (detalle != null)
            {
                detalleCliente = idServ + ";" +
                    detalle.Empresa + ";" +
                   detalle.NombreCliente + ";" +
                   detalle.RutCliente + ";" +
                   detalle.DvCliente + ";" +
                   detalle.Direccion + ";" +
                   detalle.Comuna + ";" +
                   detalle.Tarifa;

            }
            if (items.Items.Count > 0)
            {
                for (var i = 0; i < items.Items.Count; i++)
                {
                    arreglo += items.Items[i].NroDcto.ToString() + ";" + items.Items[i].Monto.ToString() + ";" + items.Items[i].Consumo.ToString() + ";" + items.Items[i].FechaFacturacion.ToString() + "@";
                }
            }
            pipe = new PipeService();
            arreglo = arreglo.Substring(0, arreglo.Length - 1);
            arreglo = detalleCliente + "#" + arreglo;
            respuesta = pipe.ComponentPipe(arreglo, "consumo");

            return Ok(respuesta);
        }



        [HttpPost]
        [AcceptVerbs("POST")]
        [ActionName("printDctoHistoPago")]
        [Route("histopago/{idEmp}/{idServ}", Order = 4, Name = "printDctoHistoPago")]
        public IHttpActionResult PrintDctoPago(int idEmp, long idServ, [FromBody] DetallePagoClienteModel detalle)
        {
            string arreglo = string.Empty;
            string respuesta = string.Empty;
            string detalleCliente = string.Empty;

            var pagging = new PaginParameterModel() { _pageSize = 30 };
            IHttpActionResult httpResp = new DocumentoController().DocumentoPago(idEmp, idServ, pagging);

            var items = ((OkNegotiatedContentResult<ResponseClient<DocumentoObj>>)httpResp).Content;

            if (detalle != null)
            {
                detalleCliente = detalle.NroCliente + ";" +
                    detalle.Empresa + ";" +
                   detalle.NombreCliente + ";" +
                   detalle.RutCliente + ";" +
                   detalle.DvCliente + ";" +
                   detalle.Direccion + ";" +
                   detalle.Comuna + ";" +
                   detalle.Tarifa;

            }
            if (items.Items.Count > 0)
            {
                for (var i = 0; i < items.Items.Count; i++)
                {
                    arreglo += items.Items[i].NroDocto.ToString() + ";" + items.Items[i].MontoPago.ToString() + ";" + items.Items[i].FechaPago.ToString() + ";" + items.Items[i].MedioPago.ToString() + "@";
                }
            }
            pipe = new PipeService();
            arreglo = arreglo.Substring(0, arreglo.Length - 1);
            arreglo = detalleCliente + "#" + arreglo;
            respuesta = pipe.ComponentPipe(arreglo, "historial");

            return Ok(respuesta);
        }
    }
}
