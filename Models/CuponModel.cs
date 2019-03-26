using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Serialization;

namespace SAESA.Models
{
    /// <summary>
    /// clase representativa con la lisrta de indoremación del cliente.
    /// </summary>
    [XmlRoot("Datos"), XmlType("Datos")]
    public class CuponModel
    {
        /// <summary>
        /// retorna la lista de información del cliente
        /// </summary>
        [XmlElement(ElementName = "InformacionServicio")]
        public List<CuponObj> ObjList { get; set; }
    }

    /// <summary>
    /// clase representativa de la información del cliente
    /// </summary>
    public class CuponObj
    {

        private string fechaVcto;
        private string fechaLectura;
        private string empresa;
        private string _urlDocumento;
        private string ordenFecha;
        private string _ordenFechaLectura;

        /// <summary>
        /// constructor del objeto
        /// </summary>
        public CuponObj() { }

        /// <summary>
        /// <see cref="Empresa"/>
        /// </summary>
        [XmlElement(ElementName = "NRO_DCTO")]
        public long NroDcto { get; set; }

        /// <summary>
        /// <see cref="DeudaTotal"/>
        /// </summary>
        [XmlElement(ElementName = "SALDO")]
        public long Saldo { get; set; }

        /// <summary>
        /// return fecha vencimiento
        /// </summary>
        [XmlElement(ElementName = "FECHA_VCTO")]
        public string FechaVcto
        {
            get
            {
                return this.fechaVcto;
            }
            set
            {
                DateTime fechaActual = new DateTime().Date;
                DateTime fechaVencimiento= Convert.ToDateTime(this.fechaVcto);

                int result = DateTime.Compare(fechaVencimiento, fechaActual);

                if (result < 0 || result == 0)
                {
                    this.ActivaPago = true;
                }
                else
                {
                    this.ActivaPago = false;
                }
                this.fechaVcto = value.Trim();
                this.ordenFecha = this.fechaVcto;
            }
        }

        public Boolean ActivaPago { get; set; }

        /// <summary>
        /// <see cref="Comuna"/>
        /// </summary>
        [XmlElement(ElementName = "NUMERO_SERVICIO")]
        public long NumeroServicio { get; set; }

        /// <summary>
        /// <see cref="Direccion"/>
        /// </summary>
        [XmlElement(ElementName = "COD_RUTA")]
        public long CodRuta { get; set; }

        /// <summary>
        /// <see cref="RutCliente"/>
        /// </summary>
        [XmlElement(ElementName = "TIPO_DOCUMENTO")]
        public long TipoDocumento { get; set; }

        /// <summary>
        /// <see cref="NombreTitular"/>
        /// </summary>
        [XmlElement(ElementName = "COD_EMPRESA")]
        public long CodEmpresa { get; set; }

        [XmlElement(ElementName = "EMPRESA")]
        public string Empresa { get { return empresa; } set { empresa = value.Trim(); } }

        [XmlElement(ElementName = "URL")]
        public string UrlDocumento
        {
            get
            {
                return _urlDocumento;
            }
            set
            {
                if (value != null)
                {
                    WebRequest webRequest = WebRequest.Create(value);
                    webRequest.Timeout = 10000;
                    webRequest.Method = "HEAD";

                    HttpWebResponse response = null;

                    try
                    {
                        response = (HttpWebResponse)webRequest.GetResponse();
                        _urlDocumento = value;
                    }
                    catch (WebException webException)
                    {
                        _urlDocumento = "";
                    }
                    finally
                    {
                        if (response != null)
                        {
                            response.Close();
                        }
                    }
                }
                else
                {
                    _urlDocumento = value;
                }
            }
        }

        [XmlElement(ElementName = "FECHA_LECTURA")]
        public string FechaLectura {
            get
            {
                return this.fechaLectura;
            }
            set
            {
                if (fechaLectura != null)
                {
                    this.OrdenFechaLectura = this.fechaLectura;
                }
                else
                {
                    this.fechaLectura = value.Trim();
                }
            }
        }

        [XmlElement(ElementName = "DESCRIPCION")]
        public string Descripcion { get; set; }

        [XmlElement(ElementName = "MONTO")]
        public long Monto { get; set; }

        [XmlElement(ElementName = "CONSUMO")]
        public string Consumo { get; set; }

        [XmlElement(ElementName = "FECHA_FACTURACION")]
        public string FechaFacturacion { get; set; }

        [XmlElement(ElementName = "FECHA_CORTE")]
        public string FechaCorte { get; set; }

        [XmlElement(ElementName = "ESTADO")]
        public string Estado { get; set; }

        [XmlElement(ElementName = "FECHA_EMISION")]
        public string FechaEmision { get; set; }

        /// <summary>
        /// <see cref="Bloqueo"/>
        /// </summary>
        [XmlElement]
        public bool Bloqueo { get; set; }

        /// <summary>
        /// <see cref="OrdenFecha"/>
        /// </summary>
        [XmlElement(ElementName = "ORDEN_REGISTRO")]
        public string OrdenFecha
        {
            get
            {
                if (fechaVcto != null)
                {
                    var soloNum = this.fechaVcto.Split('/');
                    this.ordenFecha = string.Concat(soloNum[2], soloNum[1], soloNum[0]);
                }
                return this.ordenFecha;
            }
            set
            {
                if (fechaVcto != null)
                {
                    this.ordenFecha = this.fechaVcto.Replace("/", ""); 
                } else
                {
                    this.ordenFecha = value;
                }

            }
        }

        /// <summary>
        /// <see cref="OrdenFecha"/>
        /// </summary>
        [XmlElement(ElementName = "ORDEN_LECTURA")]
        public string OrdenFechaLectura
        {
            get
            {
                if (FechaLectura != null)
                {
                    var soloNum = this.fechaLectura.Split('/');
                    this._ordenFechaLectura = string.Concat(soloNum[2], soloNum[1], soloNum[0]);
                }
                return this._ordenFechaLectura;
            }
            set
            {
                if (FechaLectura != null)
                {
                    this._ordenFechaLectura = this.FechaLectura.Replace("/", "");
                }
                else
                {
                    this._ordenFechaLectura = value;
                }

            }
        }
    }
}