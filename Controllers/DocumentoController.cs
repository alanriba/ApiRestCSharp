using Newtonsoft.Json.Linq;
using SAESA.AutoAtencion.Servicios.Services;
using SAESA.Models;
using SAESA.Models.Response;
using SAESA.Utils;
using System;
using System.Linq;
using System.Web.Http;

namespace SAESA.Controllers
{
    [RoutePrefix("api/documento")]
    public class DocumentoController : ApiController
    {
        public DocumentoModel DetalleDocumento(int idEmp, long idServ)
        {
            string strResponse = string.Empty;
            DocumentoModel dcto = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("DetalleDocumentosPagados");
                    strResponse = rCliente.BuscarCuponPagoSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue message = (JValue)jObject["mensaje"].ToString();
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

                                while (details.Contains("   "))
                                {
                                    details = details.Replace("   ", "");
                                }

                                var xmlParse = details.Replace("</MEDIO_PAGO><DESC_TIPO_DOCTO>", "</MEDIO_PAGO></InformacionServicio><InformacionServicio><DESC_TIPO_DOCTO>");
                                dcto = ser.Deserialize<DocumentoModel>(xmlParse.Trim().TrimEnd().TrimStart());
                                return dcto;
                            }
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            if (message != null)
                            {
                                return dcto;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (ArgumentException)
            {
                
            }
            return dcto;
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("documentoPago")]
        [Route("pago/{idEmp}/{idServ}", Order = 1, Name = "documentoPago")]
        public IHttpActionResult DocumentoPago(int idEmp, long idServ, [FromUri]PaginParameterModel paginParameterModel)
        {
            string strResponse = string.Empty;
            DocumentoModel dcto = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("DetalleDocumentosPagados");
                    strResponse = rCliente.BuscarCuponPagoSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue message = (JValue)jObject["mensaje"].ToString();
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

                                while (details.Contains("   "))
                                {
                                    details = details.Replace("   ", "");
                                }

                                var xmlParse = details.Replace("</MEDIO_PAGO><DESC_TIPO_DOCTO>", "</MEDIO_PAGO></InformacionServicio><InformacionServicio><DESC_TIPO_DOCTO>");
                                dcto = ser.Deserialize<DocumentoModel>(xmlParse.Trim().TrimEnd().TrimStart());

                                // conteo de filas
                                int count = dcto.ObjList.Count;

                                // El parámetro se pasa de la cadena de consulta si es nulo, entonces el valor predeterminado será pageNumber: 1
                                int currentPage = paginParameterModel.pageNumber;

                                // El parámetro se pasa de la cadena de consulta si es nulo, entonces el valor predeterminado será pageSize: 20 
                                int pageSize = paginParameterModel.PageSize;

                                int totalCount = count;
                                int totalPages = (int)Math.Ceiling(count / (double)pageSize);
                                var docItem = (from doc in dcto.ObjList.OrderBy(a => a.FechaPago)
                                               select doc).AsQueryable();

                                var item = docItem.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                                var resp = new ResponseClient<DocumentoObj>();
                                resp.Items = item.OrderByDescending(x => x.OrdenFechaPago).ToList();
                                resp.TotalPag = totalPages;
                                resp.TotalReg = totalCount;

                                return Ok(resp);
                            }
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            if (message != null)
                            {
                                return Ok(new ApiResponse() { Code = -1, Message = message.ToString() });
                            }
                            else
                            {
                                return Ok("Datos ingresados no válido");
                            }
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

        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("documentoPagoTotal")]
        [Route("pagohisto/{idEmp}/{idServ}", Order = 1, Name = "documentoPagoTotal")]
        public IHttpActionResult DocumentoPagoTotal(int idEmp, long idServ)
        {
            string strResponse = string.Empty;
            DocumentoModel dcto = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("DetalleDocumentosPagados");
                    strResponse = rCliente.BuscarCuponPagoSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue message = (JValue)jObject["mensaje"].ToString();
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

                                while (details.Contains("   "))
                                {
                                    details = details.Replace("   ", "");
                                }

                                var xmlParse = details.Replace("</MEDIO_PAGO><DESC_TIPO_DOCTO>", "</MEDIO_PAGO></InformacionServicio><InformacionServicio><DESC_TIPO_DOCTO>");
                                dcto = ser.Deserialize<DocumentoModel>(xmlParse.Trim().TrimEnd().TrimStart());
                                return Ok(dcto);
                            }
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            if (message != null)
                            {
                                return Ok(new ApiResponse() { Code = -1, Message = message.ToString() });
                            }
                            else
                            {
                                return Ok("Datos ingresados no válido");
                            }
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
