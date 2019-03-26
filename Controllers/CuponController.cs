using Newtonsoft.Json.Linq;
using SAESA.AutoAtencion.Servicios.Services;
using SAESA.Models;
using SAESA.Models.Response;
using SAESA.Utils;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SAESA.Controllers
{
    /// <summary>
    /// Class controller
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/cupon")]
    public class CuponController : ApiController
    {
        private CuponModel CuponDetalle(int idEmp, long idServ)
        {
            string strResponse = string.Empty;
            CuponModel cupon = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("TotemObtieneDocumentosPago");
                    strResponse = rCliente.BuscarCuponPagoSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue message = (JValue)jObject["mensaje"].ToString();
                        JValue sucess = (JValue)jObject["success"].ToString();
                        JValue descriptions = (JValue)jObject["descripcion"].ToString();


                        if ((result).Value.Equals("T"))
                        {
                            if (
                                (descriptions.Value != null) &&
                                (descriptions.Value.ToString() != "") &&
                                (!descriptions.Value.ToString().Equals("<DOCUMENTOS_PAGO></DOCUMENTOS_PAGO>")))
                            {
                                var ser = new Serializer();
                                var details = "<Datos>" + descriptions + "</Datos>";

                                while (details.Contains("   "))
                                {
                                    details = details.Replace("   ", "");
                                }

                                var xmlParse = details.Replace("DOCUMENTOS_PAGO", "InformacionServicio");
                                xmlParse = xmlParse.Replace("</EMPRESA><NRO_DCTO>", "</EMPRESA></InformacionServicio><InformacionServicio><NRO_DCTO>");
                                cupon = ser.Deserialize<CuponModel>(xmlParse.Trim().TrimEnd().TrimStart());

                                return cupon;
                            }
                        }
                        else if ((result).Value.Equals("F"))
                        {
                            return cupon;
                        }
                    }
                }
                else
                {
                    return cupon;
                }
            }
            catch (ArgumentException)
            {

            }
            return cupon;
        }

        public CuponModel ConsumoDetalle(int idEmp, long idServ)
        {
            string strResponse = string.Empty;
            CuponModel cupon = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("DetalleConsumoGrilla");
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

                                var xmlParse = details.Replace("</ESTADO><FECHA_LECTURA>", "</ESTADO></InformacionServicio><InformacionServicio><FECHA_LECTURA>");
                                cupon = ser.Deserialize<CuponModel>(xmlParse.Trim().TrimEnd().TrimStart());

                                return cupon;
                            }
                        }
                        else if ((result).Value.Equals("F"))
                        { 
                                return cupon;
                        }
                    }

                } else
                {
                    return cupon;
                }
            }
            catch (ArgumentException)
            {

            }
            return cupon;
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("pago")]
        [Route("pago/{idEmp}/{idServ}", Order = 1, Name = "pago")]
        public IHttpActionResult CuponPago(int idEmp, long idServ)
        {
            string strResponse = string.Empty;
            CuponModel cupon = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("DetalleDocumento");
                    strResponse = rCliente.BuscarCuponPagoSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue message = (JValue)jObject["mensaje"].ToString();
                        JValue sucess = (JValue)jObject["success"].ToString();
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

                                var xmlParse = details.Replace("</URL><EMPRESA>", "</URL></InformacionServicio><InformacionServicio><EMPRESA>");
                                cupon = ser.Deserialize<CuponModel>(xmlParse.Trim().TrimEnd().TrimStart());

                                if (cupon.ObjList.Count() > 0)
                                {
                                    var detalle = ConsumoDetalle(idEmp, idServ);
                                    var detalle2 = CuponDetalle(idEmp, idServ);

                                    
                                    if (detalle.ObjList.Count > 0)
                                    {
                                        for(var i = 0; i < cupon.ObjList.Count; i++)
                                        {
                                            for(var b = 0; b < detalle.ObjList.Count; b++)
                                            {
                                                if ((cupon.ObjList[i].NroDcto == detalle.ObjList[b].NroDcto))
                                                {
                                                    cupon.ObjList[i].TipoDocumento = detalle.ObjList[b].TipoDocumento;

                                                    if (!detalle.ObjList[b].Estado.Equals("null"))
                                                    {
                                                        cupon.ObjList[i].Estado = detalle.ObjList[b].Estado;
                                                    } else
                                                    {
                                                        cupon.ObjList[i].Estado = "--";
                                                    }
                                                }

                                                if (detalle.ObjList[b].FechaLectura != null || detalle.ObjList[b].FechaLectura != "")
                                                {
                                                    cupon.ObjList[i].FechaEmision = detalle.ObjList[b].FechaLectura;
                                                }
                                                else
                                                {
                                                    cupon.ObjList[i].FechaEmision = "--";
                                                }
                                            }


                                            for (var b = 0; b < detalle2.ObjList.Count; b++)
                                            {
                                                if ((cupon.ObjList[i].NroDcto == detalle2.ObjList[b].NroDcto))
                                                {
                                                    cupon.ObjList[i].TipoDocumento = detalle2.ObjList[b].TipoDocumento;

                                                   
                                                }
                                            }
                                        }
                                    }
                                }
                                return Ok(cupon.ObjList.OrderBy(x => x.OrdenFecha).ToList());
                            }
                            else
                            {
                                return Ok(cupon);
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
        [ActionName("pagoDetalle")]
        [Route("pago/detalle/{idEmp}/{idServ}", Order = 1, Name = "pagoDetalle")]
        public IHttpActionResult CuponDetallePago(int idEmp, long idServ)
        {
            string strResponse = string.Empty;
            CuponModel cupon = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("TotemObtieneDocumentosPago");
                    strResponse = rCliente.BuscarCuponPagoSvc("{\"p_cod_empresa\": " + idEmp + ",\"p_numero_servicio\": " + idServ + "}");

                    if (strResponse != "")
                    {
                        JObject jObject = JObject.Parse(strResponse);

                        JValue result = (JValue)jObject["resultado"].ToString();
                        JValue message = (JValue)jObject["mensaje"].ToString();
                        JValue sucess = (JValue)jObject["success"].ToString();
                        JValue descriptions = (JValue)jObject["descripcion"].ToString();


                        if ((result).Value.Equals("T"))
                        {
                            if (
                                (descriptions.Value != null) &&
                                (descriptions.Value.ToString() != "") &&
                                (!descriptions.Value.ToString().Equals("<DOCUMENTOS_PAGO></DOCUMENTOS_PAGO>")))
                            {
                                var ser = new Serializer();
                                var details = "<Datos>" + descriptions + "</Datos>";

                                while (details.Contains("   "))
                                {
                                    details = details.Replace("   ", "");
                                }

                                var xmlParse = details.Replace("DOCUMENTOS_PAGO", "InformacionServicio");
                                xmlParse = xmlParse.Replace("</EMPRESA><NRO_DCTO>", "</EMPRESA></InformacionServicio><InformacionServicio><NRO_DCTO>");
                                cupon = ser.Deserialize<CuponModel>(xmlParse.Trim().TrimEnd().TrimStart());


                                if (cupon.ObjList.Count() > 0)
                                {
                                    var detalle = ConsumoDetalle(idEmp, idServ);

                                    if (detalle.ObjList.Count > 0)
                                    {
                                        for (var i = 0; i < cupon.ObjList.Count; i++)
                                        {
                                            for (var b = 0; b < detalle.ObjList.Count; b++)
                                            {
                                                if (cupon.ObjList[i].NroDcto == detalle.ObjList[b].NroDcto)
                                                {
                                                    if (detalle.ObjList[b].FechaLectura != null || detalle.ObjList[b].FechaLectura != "")
                                                    {
                                                        cupon.ObjList[i].FechaLectura = detalle.ObjList[b].FechaLectura;
                                                    } else
                                                    {
                                                        cupon.ObjList[i].FechaLectura = "--";
                                                    }

                                                    if (detalle.ObjList[b].FechaCorte != null || detalle.ObjList[b].FechaCorte != "")
                                                    {
                                                        cupon.ObjList[i].FechaCorte = detalle.ObjList[b].FechaCorte;
                                                    }
                                                    else
                                                    {
                                                        cupon.ObjList[i].FechaCorte = "--";
                                                    }

                                                    if (detalle.ObjList[b].FechaCorte != null || detalle.ObjList[b].FechaCorte != "")
                                                    {
                                                        cupon.ObjList[i].FechaCorte = detalle.ObjList[b].FechaCorte;
                                                    }
                                                    else
                                                    {
                                                        cupon.ObjList[i].FechaCorte = "--";
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }

                                return Ok(cupon.ObjList.OrderBy(x => x.OrdenFecha));
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
        [ActionName("consumo")]
        [Route("consumo/{idEmp}/{idServ}", Order = 1, Name = "consumo")]
        public IHttpActionResult HistorialConsumo(int idEmp, long idServ, [FromUri]PaginParameterModel paginParameterModel)
        {
            string strResponse = string.Empty;
            CuponModel cupon = null;
            try
            {
                if (idEmp != 0 && idServ != 0)
                {
                    CuponService rCliente = new CuponService("DetalleConsumoGrilla");
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

                                var xmlParse = details.Replace("</ESTADO><FECHA_LECTURA>", "</ESTADO></InformacionServicio><InformacionServicio><FECHA_LECTURA>");
                                cupon = ser.Deserialize<CuponModel>(xmlParse.Trim().TrimEnd().TrimStart());

                                // conteo de filas
                                int count = cupon.ObjList.Count;

                                // El parámetro se pasa de la cadena de consulta si es nulo, entonces el valor predeterminado será pageNumber: 1
                                int currentPage = paginParameterModel.pageNumber;

                                // El parámetro se pasa de la cadena de consulta si es nulo, entonces el valor predeterminado será pageSize: 20 
                                int pageSize = paginParameterModel.PageSize;

                                int totalCount = count;
                                int totalPages = (int)Math.Ceiling(count / (double)pageSize);
                                var docItem = (from doc in cupon.ObjList.OrderBy(a => a.FechaFacturacion)
                                               select doc).AsQueryable();

                                var item = docItem.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                                var resp = new ResponseClient<CuponObj>();
                                resp.Items = item.OrderByDescending(x => x.OrdenFechaLectura).ToList();
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
    }
}
