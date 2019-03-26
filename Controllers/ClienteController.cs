using Newtonsoft.Json.Linq;
using SAESA.AutoAtencion.Servicios.Services;
using SAESA.Models;
using SAESA.Models.Response;
using SAESA.Utils;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SAESA.Controllers
{
    /// <summary>
    /// Class controller
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/cliente")]
    public class ClienteController : ApiController
    {
        /// <summary>
        /// metodo para buscar cliente
        /// </summary>
        /// <param name="idEmp"></param>
        /// <param name="idCli"></param>
        /// <returns>lista de servicios</returns>
        /// <response code="200">successful operation</response>
        /// <summary>
        /// 
        [SwaggerOperation("BuscadorCliente")]
        [SwaggerResponse(statusCode: 200, description: "Operación Correcta")]
        [SwaggerResponse(statusCode: 401, description: "Informacion no encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Error en el servidor")]
        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("search")]
        [Route("{idEmp}/{idServ}", Order = 1, Name = "search")]
        public IHttpActionResult SearchClient(int idEmp, long idServ= 0)
        {
            string strResponse = string.Empty;
            ClienteModel customer = new ClienteModel();
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    ClienteService rCliente = new ClienteService("ResumenCuenta");
                    strResponse = rCliente.BuscarClienteSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);
                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue descriptions = (JValue)jObject["descripcion"].ToString();

                        if ((result).Value.Equals("T"))
                        {
                            if (
                                (descriptions.Value != null) &&
                                (descriptions.Value.ToString() != "") &&
                                (!descriptions.Value.ToString().Equals("<InformacionServicio></InformacionServicio>")))
                            {
                                var ser = new Serializer();
                                var details = "<Datos>" + descriptions + "</Datos>";

                                //while (details.Contains(" "))
                                //{
                                //    details = details.Replace(" ", "");
                                //}
                                var xmlParse = details.Replace("</URL><EMPRESA>", "</URL></InformacionServicio><InformacionServicio><EMPRESA>");
                                customer = ser.Deserialize<ClienteModel>(xmlParse);
                            }
                            return Ok(customer);
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            return Ok("Datos ingresados no válido");
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                return InternalServerError();
            }
            return InternalServerError();
        }

        [SwaggerOperation("BuscadorCliente")]
        [SwaggerResponse(statusCode: 200, description: "Operación Correcta")]
        [SwaggerResponse(statusCode: 401, description: "Informacion no encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Error en el servidor")]
        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("clienteDetalle")]
        [Route("detalle/{idEmp}/{idServ}", Order = 1, Name = "clienteDetalle")]
        public IHttpActionResult SearchClientDetails(int idEmp, long idServ)
        {
            string strResponse = string.Empty;
            ClienteModel customer = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    ClienteService rCliente = new ClienteService("ObtieneInformacionCliente");
                    strResponse = rCliente.BuscarClienteSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue descriptions = (JValue)jObject["descripcion"].ToString();

                        if ((result).Value.Equals("T"))
                        {
                            if (
                                (descriptions.Value != null) &&
                                (descriptions.Value.ToString() != "") &&
                                (!(descriptions.Value.ToString().Equals("<InformacionServicio></InformacionServicio>"))))
                            {
                                var ser = new Serializer();
                                var details = "<Datos>" + descriptions + "</Datos>";

                                //while (details.Contains(" "))
                                //{
                                //    details = details.Replace(" ", "");
                                //}
                                var xmlParse = details.Replace("</CHK_INF_CELULAR><ID_EMPRESA>", "</CHK_INF_CELULAR></InformacionServicio><InformacionServicio><ID_EMPRESA>");
                                customer = ser.Deserialize<ClienteModel>(xmlParse);
                            }
                            return Ok(customer);
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            return Ok("Datos ingresados no válido");
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                return InternalServerError();
            }
            return InternalServerError();
        }

        [SwaggerOperation("BuscadorCliente")]
        [SwaggerResponse(statusCode: 200, description: "Operación Correcta")]
        [SwaggerResponse(statusCode: 401, description: "Informacion no encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Error en el servidor")]
        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("clienteRut")]
        [Route("detalleRut/{idEmp}/{rutCli}/", Order = 1, Name = "clienteRut")]
        public IHttpActionResult SearchClientDetailsPerRut(string rutCli, int idEmp)
        {
            string strResponse = string.Empty;
            ClienteModel customer = null;
            try
            {
                if (idEmp != 0 && rutCli != "")
                {
                    rutCli = rutCli.Replace('.',' ');
                    rutCli = rutCli.Replace('-',' ');
                    rutCli = rutCli.Substring(0, rutCli.Length - 1);
                    rutCli = rutCli.Replace(" ", "");
                    ClienteService rCliente = new ClienteService("BuscadorCliente");
                    strResponse = rCliente.BuscarClienteSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_rut_cliente\": " + rutCli + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue descriptions = (JValue)jObject["descripcion"].ToString();

                        if ((result).Value.Equals("T"))
                        {
                            if (
                                (descriptions.Value != null) &&
                                (descriptions.Value.ToString() != "") &&
                                (!(descriptions.Value.ToString().Equals("<DatosServicios></DatosServicios>"))))
                            {
                                var ser = new Serializer();
                                var details = "<Datos>" + descriptions + "</Datos>";
                              
                                details = details.Replace("DatosServicios", "InformacionServicio");
                                
                                var xmlParse = details.Replace("</DIRECCION>   <EMPRESA>", "</DIRECCION></InformacionServicio><InformacionServicio><EMPRESA>");
                                customer = ser.Deserialize<ClienteModel>(xmlParse);
                            }
                            return Ok(customer);
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            return Ok("Datos ingresados no válido");
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                return InternalServerError();
            }
            return InternalServerError();
        }

        [SwaggerOperation("BuscadorCliente")]
        [SwaggerResponse(statusCode: 200, description: "Operación Correcta")]
        [SwaggerResponse(statusCode: 401, description: "Informacion no encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Error en el servidor")]
        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("BuscadorDireccion")]
        [Route("buscadorDireccion/{idEmp}/{idCom}/{idCalle}/{num}/{dpto}", Order = 1, Name = "buscadorDireccion")]
        public IHttpActionResult SearchClientDetailsPerLocation(int idEmp, int idCom, int idCalle, int num, string dpto, [FromUri]PaginParameterModel paginParameterModel)
        {

            string dptoRq = (dpto == "0")? "" : dpto.ToString();
            string nroRq = (num.ToString() == "0") ? "0" : num.ToString();
            string strResponse = string.Empty;
            ClienteModel customer = null;
            ResponseClient<ClienteObj> resp = null;
            try
            {
                if (idEmp != 0 && idCom != 0 && idCalle != 0)
                {
                    ClienteService rCliente = new ClienteService("BuscadorDireccion");
                    strResponse = rCliente.BuscarClienteSvc("{\"p_cod_empresa\":  " + idEmp + " ,\"p_cod_comuna\": " + idCom + " ,\"p_cod_calle\":  " + idCalle + ", \"p_numero\": " + nroRq.Trim() + ", \"p_departamento\": \"" + dptoRq.Trim() + "\" }");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue descriptions = (JValue)jObject["descripcion"].ToString();

                        if ((result).Value.Equals("T"))
                        {
                            if (
                                (descriptions.Value != null) &&
                                (descriptions.Value.ToString() != "") &&
                                (!(descriptions.Value.ToString().Equals("<DatosServicios></DatosServicios>"))))
                            {
                                var ser = new Serializer();
                                var details = "<Datos>" + descriptions + "</Datos>";
                                
                                details = details.Replace("DatosServicios", "InformacionServicio");
                                var xmlParse = details.Replace("</DIRECCION>   <EMPRESA>", "</DIRECCION></InformacionServicio><InformacionServicio><EMPRESA>");
                                
                                customer = ser.Deserialize<ClienteModel>(xmlParse);

                                // conteo de filas
                                int count = customer.ObjList.Count;

                                // El parámetro se pasa de la cadena de consulta si es nulo, entonces el valor predeterminado será pageNumber: 1
                                int currentPage = paginParameterModel.pageNumber;

                                // El parámetro se pasa de la cadena de consulta si es nulo, entonces el valor predeterminado será pageSize: 20 
                                int pageSize = 10000;

                                int totalCount = count;
                                int totalPages = (int)Math.Ceiling(count / (double)pageSize);
                                var docItem = (from doc in customer.ObjList.OrderBy(a => a.NumeroServicio)
                                               select doc).AsQueryable();

                                var item = docItem.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                                resp = new ResponseClient<ClienteObj>();
                                resp.Items = item.OrderByDescending(x => x.NumeroServicio).ToList();
                                resp.TotalPag = totalPages;
                                resp.TotalReg = totalCount;
                                return Ok(resp);
                            }
                             
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            return Ok(new ResponseClient<ClienteObj>());
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                return InternalServerError();
            }
            return InternalServerError();
        }
    }
}
