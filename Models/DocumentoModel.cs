using System.Collections.Generic;
using System.Xml.Serialization;

namespace SAESA.Models
{
    [XmlRoot("Datos"), XmlType("Datos")]
    public class DocumentoModel
    {
        /// <summary>
        /// retorna la lista de información del cliente
        /// </summary>
        [XmlElement(ElementName = "InformacionServicio")]
        public List<DocumentoObj> ObjList { get; set; }
    }

    /// <summary>
    /// clase representativa de la información del cliente
    /// </summary>
    public class DocumentoObj
    {

        private string _ordenFechaPago;
        private string fechaPago;

        /// <summary>
        /// constructor del objeto
        /// </summary>
        public DocumentoObj() { }

        /// <summary>
        /// <see cref="Empresa"/>
        /// </summary>
        [XmlElement(ElementName = "DESC_TIPO_DOCTO")]
        public string DescTipoDocto { get; set; }

        /// <summary>
        /// <see cref="NumeroServicio"/>
        /// </summary>
        [XmlElement(ElementName = "NUMERO_DOCUMENTO")]
        public int NroDocto { get; set; }

        /// <summary>
        /// <see cref="Comuna"/>
        /// </summary>
        [XmlElement(ElementName = "MONTO_PAGO")]
        public long MontoPago { get; set; }

        /// <summary>
        /// <see cref="Direccion"/>
        /// </summary>
        [XmlElement(ElementName = "FEC_PAGO")]
        public string FechaPago {
            get
            {
                return this.fechaPago;
            }
            set
            {
                if (fechaPago != null)
                {
                    this.OrdenFechaPago = this.fechaPago;
                }
                else
                {
                    this.fechaPago = value.Trim();
                }
            }
        }

        /// <summary>
        /// <see cref="RutCliente"/>
        /// </summary>
        [XmlElement(ElementName = "MEDIO_PAGO")]
        public string MedioPago { get; set; }


        /// <summary>
        /// <see cref="RutCliente"/>
        /// </summary>
        [XmlElement(ElementName = "FECHA_EMISION")]
        public string FechaEmision { get; set; }

        [XmlElement(ElementName = "ORDEN_FECHA_PAGO")]
        public string OrdenFechaPago
        {
            get
            {
                if (FechaPago != null)
                {
                    var soloNum = this.FechaPago.Split('/');
                    this._ordenFechaPago = string.Concat(soloNum[2], soloNum[1], soloNum[0]);
                }
                return this._ordenFechaPago;
            }
            set
            {
                if (FechaPago != null)
                {
                    this._ordenFechaPago = this.FechaPago.Replace("/", "");
                }
                else
                {
                    this._ordenFechaPago = value;
                }

            }
        }

    }
}
