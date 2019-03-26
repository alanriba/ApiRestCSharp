using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SAESA.Utils
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Serializer

    {
        /// <summary>
        /// recibe una cadena de texto que contiene un xml y lo devuelte en objeto de tipo T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public T Deserialize<T>(string input) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Serialize<T>(T ObjectToSerialize)

        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
    }

}