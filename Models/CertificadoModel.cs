namespace SAESA.Models
{
    public class CertificadoModel
    {
        public int idEmp { get; set; } // Código de la empresa
        public long NumCli { get; set; } // Servicio: Numero de servicio a consultar
        public string FechaInicio { get; set; } // Fecha inicio: Fecha de inicio del certificado en formato dd-mm-yyyy
        public string FechaTermino { get; set; } // Fecha fin: Fecha de fin del certificado en formato dd-mm-yyyy
        public int TipCert { get; set; } // Id del certificado: Corresponde al id del subtipo de los certificados, para ver los id ejecutar la siguiente consulta en el esquema de SES_DES:
    }
}
