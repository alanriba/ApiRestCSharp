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
    [RoutePrefix("api/generate")]
    public class CertificadoController : ApiController
    {
        private PipeService pipe;

        [HttpPost]
        [AcceptVerbs("POST")]
        [ActionName("generarCertificado")]
        [Route("certificado", Order = 1, Name = "generarCertificado")]
        public IHttpActionResult GenerarCertificado([FromBody] CertificadoModel certificado)
        {
            pipe = new PipeService();
            var oConfig = Configuracion.Instancia();
            string respuesta = string.Empty;

            if (certificado != null)
            {
                var fIni = certificado.FechaInicio.Replace('/', '-');
                var fFin = certificado.FechaTermino.Replace('/', '-');

                DateTime fechaInicio = new DateTime();
                DateTime fechaTermino = new DateTime();

                fechaInicio = Convert.ToDateTime(fIni);
                fechaTermino = Convert.ToDateTime(fFin);

                

                string urlCertificado = oConfig.IpEndPoint + ':' + oConfig.PortEndPoint + oConfig.UrlRelativa + 
                    "SaesaCertificados/" + 
                    certificado.idEmp + "/" +  
                    certificado.NumCli + "/" +
                    fechaInicio.ToString("dd-MM-yyyy") + "/" + 
                    fechaTermino.ToString("dd-MM-yyyy") + '/' + 
                    certificado.TipCert + "/WS";
                respuesta = pipe.ComponentPipeDownLoadFile(urlCertificado);
            }
            return Ok(respuesta);
        }


        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("getPrueba")]
        [Route("certificado", Order = 1, Name = "getPrueba")]
        public IHttpActionResult PruebaCertifido()
        {
          
            return Ok("OK");
        }
    }
}
