using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAESA.Models
{
    /// <summary>
    /// Clase que permite convertir la respuesta en objeto
    /// </summary>
    public class ConvertJsonToModel
    {
        private readonly JToken result;
        private readonly JToken message;
        private readonly JToken success;
        private readonly JToken descriptions;

        /// <summary>
        /// Contructor principal de la clase
        /// </summary>
        public ConvertJsonToModel()
        { }


        public ConvertJsonToModel(JToken r, JToken m, JToken s, JToken d)
        {
            this.result = r;
            this.message = m;
            this.success = s;
            this.descriptions = d;
        }

        /// <summary>
        /// detalle del resultado
        /// </summary>
        public JToken Result
        {
            set
            {
                value = result;
            }
            get
            {
                return result;
            }
        }

        /// <summary>
        /// mensaje de estado
        /// </summary>
        public JToken Message
        {
            get => message;
            set => value = message;
        }

        /// <summary>
        /// resultado de mensaje
        /// </summary>
        public JToken Sucess
        {
            get => success;
            set => value = success;
        }

        /// <summary>
        /// descripción del mensaje
        /// </summary>
        public JToken Descriptions
        {
            get => descriptions;
            set => value = descriptions;
        }

    }
}