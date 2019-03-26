using SAESA.Models;
using SAESA.Utils;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SAESA.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/saesa")]
    public class SaesaController : ApiController
    {
        public int Timeout { get; set; }

        private UseReportService useReportService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("seguimiento")]
        [Route("seguimiento/{session}/{rut}/{accion}", Order = 1, Name = "seguimiento")]
        public string RegistroSeguimiento(string session, string rut, string accion)
        {
            // save(session, rut, accion);
            return "ok";
        }

        [HttpPost]
        [ActionName("seguimiento")]
        [Route("usabilidad")]
        public IHttpActionResult RegistroUsabilidad([FromBody] UsabilidadModel usabilidad)
        {
            guardarUsabilidad(usabilidad);
            return Ok("OK");
        }

        public void guardarUsabilidad(UsabilidadModel usabilidad)
        {
            string msg = string.Empty;
            ReportModel reportData = new ReportModel();
            useReportService = new UseReportService();
            string fecha = System.DateTime.Now.ToString("yyyyMMdd");
            string hora = System.DateTime.Now.ToString("HH:mm:ss");
            string path = HttpContext.Current.Request.MapPath("~/log/" + fecha + ".txt");

            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(fecha + ';' + hora + ';' + usabilidad.session + ';' + usabilidad.rut + ';' + usabilidad.accion);
            sw.Flush();
            sw.Close();

            try
            {
                ServicioGlobal trx = new ServicioGlobal();

                reportData.HoraInicio = Convert.ToDateTime(usabilidad.horaIni).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                reportData.HoraTermino = Convert.ToDateTime(usabilidad.horaFin).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                reportData.Session = usabilidad.session;
                reportData.Modulo = trx.getMensaje(usabilidad.modulo);
                reportData.Accion = trx.getMensaje(usabilidad.accion);
                reportData.Resultado = trx.getMensaje(usabilidad.resultado); //Cierre
                reportData.Rut = usabilidad.rut.Replace(".", "");
                reportData.Empresa = usabilidad.empresa;
                reportData.NroServicio = usabilidad.nroServicio;
                reportData.Empresa = usabilidad.empresa;
                
                useReportService.sendData(reportData);

                if (useReportService.Resultado.CodigoError == 0)
                {
                    msg = string.Join("|", useReportService.Resultado.Data[0]);
                    log.Info("Uso, Res envio data pipeService: " + msg);
                }
                else
                {
                    msg = string.Join("|", useReportService.Resultado.Data[0]);
                    log.Error("Uso, error: " + msg);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void save(string session, string rut, string accion)
        {
            string msg = string.Empty;
            ReportModel reportData = new ReportModel();
            useReportService = new UseReportService();
            string fecha = System.DateTime.Now.ToString("yyyyMMdd");
            string hora = System.DateTime.Now.ToString("HH:mm:ss");
            string path = HttpContext.Current.Request.MapPath("~/log/" + fecha + ".txt");

            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine(fecha + ';' + hora + ';' + session + ';' + rut + ';' + accion);
            sw.Flush();
            sw.Close();

            string horaInicio =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            try
            {
                ServicioGlobal trx = new ServicioGlobal();

                string detMensaje = trx.getMensaje(accion);

             /* 
                reportData.Rut = rut;
                reportData.Nombre = session;
                reportData.Sexo = "s";
                reportData.DesMovimiento = "AutoServicio";
                reportData.Movimiento = accion;
                reportData.HoraTermino = horaInicio;
                reportData.HoraInicio = horaInicio;
                reportData.MovimientoRes = "AutoServicio";
                reportData.Detalle = detMensaje; // Acción
                reportData.OperacionRealizada = "V";
                reportData.EmisionTurno = "0"; 
                */

                useReportService.sendData(reportData);

                if (useReportService.Resultado.CodigoError == 0)
                {
                    msg = string.Join("|", useReportService.Resultado.Data[0]);
                    log.Info("Uso, Res envio data pipeService: " + msg);
                }
                else
                {
                    msg = string.Join("|", useReportService.Resultado.Data[0]);
                    log.Error("Uso, error: " + msg);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
