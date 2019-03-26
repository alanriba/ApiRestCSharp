using SAESA.AutoAtencion.Framework.Configuracion;
using SAESA.AutoAtencion.Servicios.Pago;
using SAESA.AutoAtencion.Servicios.Services;
using SAESA.Models;
using System.Web.Http;

namespace SAESA.Controllers
{
    [RoutePrefix("api/pago")]
    public class PagoController : ApiController
    {

        private Configuracion oConfig = Configuracion.Instancia();


        [HttpPost]
        [Route("tbk", Order = 1, Name = "pagoTbk")]
        [ActionName("pagoTbk")]
        public IHttpActionResult PagoTbk([FromBody] DetallePagoClienteModel detalle)
        {
            Movimiento movimiento = null;
            string msg = string.Empty;

            if (detalle != null)
            {
                movimiento = new Movimiento();
                msg = movimiento.InsertarPago(detalle.RutCliente, detalle.DvCliente, detalle.TotalCancelado.Replace(".", ""), detalle.NroCliente.ToString());
            }
            return Ok(msg);
        }

        [HttpGet]
        [Route("cierre", Order = 1, Name = "cierreTbk")]
        [ActionName("cierreTbk")]
        public IHttpActionResult CierreTbk()
        {
            Movimiento movimiento = null;
            string msg = string.Empty;

            movimiento = new Movimiento();
            msg = movimiento.ValidarTbk(ServicioPago.Comandos.CierreTbk, oConfig.IdKiosko);
            PipeService pipe = new PipeService();

            var algo = msg.Split('~');

            var respuesta = pipe.ComponentPipePrintVoutcher("", algo[2]);
            Log.Write("Usabilidad", msg, Evento.Response, ServicioPago.Comandos.PagoTbk);
            return Ok(new ApiResponse() { Code = 0, Message = "OK".ToString() });
        }

        [HttpGet]
        [Route("inicializacion", Order = 1, Name = "inicializacionTbk")]
        [ActionName("inicializacionTbk")]
        public IHttpActionResult InicializacionTbk()
        {
            Movimiento movimiento = null;
            string msg = string.Empty;

            movimiento = new Movimiento();
            msg = movimiento.ValidarTbk(ServicioPago.Comandos.IniTbk, oConfig.IdKiosko);

            return Ok(msg);
        }


        [HttpGet]
        [Route("carga", Order = 1, Name = "cargaLlavesTbk")]
        [ActionName("cargaLlavesTbk")]
        public IHttpActionResult CargaLlavesTbk()
        {
            Movimiento movimiento = null;
            string msg = string.Empty;

            movimiento = new Movimiento();
            msg = movimiento.ValidarTbk(ServicioPago.Comandos.CargaLlavesTbk, oConfig.IdKiosko);

            return Ok(msg);
        }

        [HttpGet]
        [Route("pooling", Order = 1, Name = "verificarEstado")]
        [ActionName("verificarEstado")]
        public IHttpActionResult PoolingTbk()
        {
            Movimiento movimiento = null;
            string msg = string.Empty;

            movimiento = new Movimiento();
            msg = movimiento.ValidarTbk(ServicioPago.Comandos.Pooling, oConfig.IdKiosko);

            return Ok(msg);
        }

        [HttpGet]
        [Route("anular", Order = 1, Name = "anularPagoTbk")]
        [ActionName("anularPagoTbk")]
        public IHttpActionResult AnularPagoTbk()
        {
            Movimiento movimiento = null;
            string msg = string.Empty;

            movimiento = new Movimiento();
            msg = movimiento.ValidarTbk(ServicioPago.Comandos.AnulaPago, oConfig.IdKiosko);

            // Retorno de información desdee TBK
            // Debito : 07
            // Otro (Visa - Crédito): Código

            return Ok(msg);
        }

        [HttpGet]
        [Route("ultima", Order = 1, Name = "ultimaVentaTbk")]
        [ActionName("ultimaVentaTbk")]
        public IHttpActionResult UltimaVentaTbk()
        {
            Movimiento movimiento = null;
            string msg = string.Empty;

            movimiento = new Movimiento();
            msg = movimiento.ValidarTbk(ServicioPago.Comandos.UltimaVenta, oConfig.IdKiosko);

            return Ok(msg);
        }

    }
}
