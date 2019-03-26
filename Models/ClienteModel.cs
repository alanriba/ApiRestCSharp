using System.Collections.Generic;
using System.Xml.Serialization;

namespace SAESA.Models
{
    /// <summary>
    /// clase representativa con la lisrta de indoremación del cliente.
    /// </summary>
    [XmlRoot("Datos"), XmlType("Datos")]
    public class ClienteModel
    {
        /// <summary>
        /// retorna la lista de información del cliente
        /// </summary>
        [XmlElement(ElementName = "InformacionServicio")]
        public List<ClienteObj> ObjList { get; set; }
    }

    /// <summary>
    /// clase representativa de la información del cliente
    /// </summary>
    public class ClienteObj
    {
        /// <summary>
        /// constructor del objeto
        /// </summary>
        public ClienteObj() { }

        /// <summary>
        /// <see cref="Empresa"/>
        /// </summary>
        [XmlElement(ElementName = "EMPRESA")]
        public string Empresa { get; set; }

        /// <summary>
        /// <see cref="NumeroServicio"/>
        /// </summary>
        [XmlElement(ElementName = "NUMERO_SERVICIO")]
        public long NumeroServicio { get; set; }

        /// <summary>
        /// <see cref="Comuna"/>
        /// </summary>
        [XmlElement(ElementName = "COMUNA")]
        public string Comuna { get; set; }

        /// <summary>
        /// <see cref="Direccion"/>
        /// </summary>
        [XmlElement(ElementName = "DIRECCION")]
        public string Direccion { get; set; }

        /// <summary>
        /// <see cref="RutCliente"/>
        /// </summary>
        [XmlElement(ElementName = "RUT_CLIENTE")]
        public string RutCliente { get; set; }

        /// <summary>
        /// <see cref="RutTitular"/>
        /// </summary>
        [XmlElement(ElementName = "RUT_TITULAR")]
        public string RutTitular { get; set; }

        /// <summary>
        /// <see cref="DvRutTitular"/>
        /// </summary>
        [XmlElement(ElementName = "DV_RUT_TITULAR")]
        public string Dv { get; set; }




        /// <summary>
        /// <see cref="NombreTitular"/>
        /// </summary>
        [XmlElement(ElementName = "NOMBRE_TITULAR")]
        public string NombreTitular { get; set; }

        /// <summary>
        /// <see cref="Tarifa"/>
        /// </summary>
        [XmlElement(ElementName = "TARIFA")]
        public string Tarifa { get; set; }

        /// <summary>
        /// <see cref="DeudaTotal"/>
        /// </summary>
        [XmlElement(ElementName = "DEUDA_TOTAL")]
        public long DeudaTotal { get; set; }

        /// <summary>
        /// <see cref="DocumentosImpagos"/>
        /// </summary>
        [XmlElement(ElementName = "DCTOS_IMPAGOS")]
        public int DocumentosImpagos { get; set; }

        /// <summary>
        /// <see cref="FechaCorte"/>
        /// </summary>
        [XmlElement(ElementName = "FECHA_CORTE")]
        public string FechaCorte { get; set; }


        /// <summary>
        /// <see cref="CodAreaTelefono"/>
        /// </summary>
        [XmlElement(ElementName = "COD_AREA_TELEFONO")]
        public int CodAreaTelefono { get; set; }

        /// <summary>
        /// <see cref="CodAreaTelefono"/>
        /// </summary>
        [XmlElement(ElementName = "TELEFONO")]
        public int Telefono { get; set; }


        /// <summary>
        /// <see cref="CodCelular"/>
        /// </summary>
        [XmlElement(ElementName = "COD_CELULAR")]
        public int CodCelular { get; set; }

        /// <summary>
        /// <see cref="Celular"/>
        /// </summary>
        [XmlElement(ElementName = "CELULAR")]
        public int Celular { get; set; }

        /// <summary>
        /// <see cref="Correo"/>
        /// </summary>
        [XmlElement(ElementName = "CORREO")]
        public string Correo { get; set; }

        /// <summary>
        /// <see cref="CHK_BOLETA"/>
        /// </summary>
        [XmlElement(ElementName = "CHK_BOLETA")]
        public string ChkBoleta { get; set; }

        /// <summary>
        /// <see cref="CHK_OTRAS_VENTAS"/>
        /// </summary>
        [XmlElement(ElementName = "CHK_OTRAS_VENTAS")]
        public string ChkOtrasVentas { get; set; }

        /// <summary>
        /// <see cref="CHK_INF_CELULAR"/>
        /// </summary>
        [XmlElement(ElementName = "CHK_INF_CELULAR")]
        public string ChkInfCelular { get; set; }

        [XmlElement(ElementName = "NOMBRE_SERVICIO")]
        public string NombreServicio { get; set; }

    }
}