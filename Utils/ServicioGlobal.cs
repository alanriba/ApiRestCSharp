using Microsoft.VisualBasic;
using System;

namespace SAESA.Utils
{
    public class ServicioGlobal
    {
        public const string Header = "TP10PP";

        public enum Posicion
        {
            HEAD_LEN = 6,
            LEN_LEN = 5,
            CMD_LEN = 3,
            CKS_LEN = 4,
            FOOT_LEN = 5,                                                                // 4(Cks) + 1(@)
            MAX_DAT_LEN = 8192,                                                          // Largo maximo Data
            FIX_LEN = (HEAD_LEN + LEN_LEN + CMD_LEN + FOOT_LEN),
            LEN_POS = (HEAD_LEN),                                                        // Posición Largo en Mensaje
            CMD_POS = (HEAD_LEN + LEN_LEN),                                              // Posición Comando en Mensaje
            DAT_POS = (HEAD_LEN + LEN_LEN + CMD_LEN),                                    // Posición Data en Mensaje
            MIN_MSG_LEN = (HEAD_LEN + LEN_LEN + CMD_LEN + FOOT_LEN),                     // Largo Min Mensaje
            MAX_MSG_LEN = (HEAD_LEN + LEN_LEN + CMD_LEN + MAX_DAT_LEN + FOOT_LEN)        // Largo Max Mensaje
        }

        public enum ValidaMsg
        {
            MSGMIN = 12001,   // MSG: largo < permitido
            MSGMAX = 12002,   // MSG: largo > permitido
            MSGFMT = 12003,   // MSG: formato
            MSGLEN = 12004,   // MSG: tamaño string recv
            MSGCMD = 12005,   // MSG: comando no válido
            MSGCKS = 12006,   // MSG: checksum no válido
            TIMOUT = 11190,   // Timeout
            GENSCS = 12099    // Error general
        }

        public enum ValidaMsgServer
        {
            MSGMIN = 11501,   // MSG: largo < permitido
            MSGMAX = 11502,   // MSG: largo > permitido
            MSGFMT = 11503,   // MSG: formato
            MSGLEN = 11504,   // MSG: tamaño string recv
            MSGCMD = 11505,   // MSG: comando no válido
            MSGCKS = 11506,   // MSG: checksum no válido
            MSGDAT = 11507,   // MSG: número de data no válida
            ERRTBK = 11541,   // MSG: error en transbank
            GENSCS = 11599    // Error general
        }

        public static bool IsChecksumValid(string receiveText)
        {
            return !(Strings.Mid(receiveText, Convert.ToInt32(Strings.Mid(receiveText, 7, 5)) - 5 + 1, 4) != new ServicioGlobal().Checksum(Strings.Mid(receiveText, 1, receiveText.Length - 5)));
        }

        private string Checksum(string data)
        {
            long cks = 0;
            for (int i = 1; i <= data.Length; i++)
            {
                cks -= Strings.Asc(Strings.Mid(data, i, 1));
            }
            return Strings.Right(Conversion.Hex(cks), 4).PadLeft(4, '0');
        }

        public string GetError(int command)
        {
            switch (command)
            {
                case (int)ValidaMsg.MSGMIN:
                case (int)ValidaMsgServer.MSGMIN:
                    return "Largo de mensaje < permitido";
                case (int)ValidaMsg.MSGMAX:
                case (int)ValidaMsgServer.MSGMAX:
                    return "Largo de mensaje > permitido";
                case (int)ValidaMsg.MSGFMT:
                case (int)ValidaMsgServer.MSGFMT:
                    return "Error de formato en mensaje";
                case (int)ValidaMsg.MSGLEN:
                case (int)ValidaMsgServer.MSGLEN:
                    return "Error de tamaño de mensaje";
                case (int)ValidaMsg.MSGCMD:
                case (int)ValidaMsgServer.MSGCMD:
                    return "Comando no válido";
                case (int)ValidaMsg.MSGCKS:
                case (int)ValidaMsgServer.MSGCKS:
                    return "Checksum no válido";
                case (int)ValidaMsgServer.MSGDAT:
                    return "Parámetros no válidos";
                case (int)ValidaMsgServer.GENSCS:
                    return "Error general";
                case (int)ValidaMsg.TIMOUT:
                    return "Timeout Servidor";
                default: return "Error General";
            }
        }

        public string getMensaje(string codigo)
        {
            if (codigo == "A001") { return "Búsqueda directa"; }
            if (codigo == "A002") { return "Búsqueda directa OK"; }
            if (codigo == "A003") { return "Búsqueda directa NO OK"; }
            if (codigo == "A004") { return "Ingreso Búsqueda directa"; }

            if (codigo == "A000") { return "Ingreso por Búsqueda"; }
            if (codigo == "A100") { return "Búsqueda por RUT"; }
            if (codigo == "A101") { return "Búsqueda por RUT OK"; }
            if (codigo == "A102") { return "Búsqueda por RUT NO OK"; }

            if (codigo == "A200") { return "Búsqueda por Dirección"; }
            if (codigo == "A201") { return "Búsqueda por Dirección OK"; }
            if (codigo == "A202") { return "Búsqueda por Dirección NO OK"; }

            // Pago de Cuentas
            if (codigo == "A300") { return "Pago de Cuentas"; }
            if (codigo == "A301") { return "Búsqueda por Pago de Cuentas OK"; }
            if (codigo == "A302") { return "Búsqueda por Pago de Cuentas NO OK"; }
            if (codigo == "A303") { return "Generar Cupón de Pago"; }
            if (codigo == "A304") { return "Cupón de Pago Generado OK"; }
            if (codigo == "A305") { return "Cupón de Pago Generado NO OK"; }

            // Boletas
            if (codigo == "A400") { return "Boletas"; }
            if (codigo == "A401") { return "Búsqueda por Boletas OK"; }
            if (codigo == "A402") { return "Búsqueda por boletas NO OK"; }
            if (codigo == "A403") { return "Previsualizar Boletas"; }
            if (codigo == "A404") { return "Imprimir Boletas"; }
            if (codigo == "A405") { return "Imprimir Boletas OK"; }
            if (codigo == "A406") { return "Imprimir Boletas NO OK"; }
            if (codigo == "A407") { return "Enviar Boletas por Email"; }
            if (codigo == "A408") { return "Enviar Boletas por Email OK"; }

            // Cupón de Pago
            if (codigo == "A500") { return "Cupón de Pago"; }
            if (codigo == "A501") { return "Ingreso Cupón de Pago"; }
            if (codigo == "A502") { return "Salida Cupón de Pago"; }
            if (codigo == "A503") { return "OK"; }
            if (codigo == "A504") { return "NO OK"; }
            if (codigo == "A505") { return "Búsqueda por Cupón de Pago OK"; }
            if (codigo == "A506") { return "Búsqueda por Cupón de Pago NO OK"; }
            if (codigo == "A507") { return "Generar cupón de Pago"; }
            if (codigo == "A508") { return "Generar cupón de Pago OK"; }
            if (codigo == "A509") { return "Generar cupón de Pago NO OK"; }

            // Consumo Eléctrico
            if (codigo == "A600") { return "Consumo Eléctrico"; }
            if (codigo == "A601") { return "Ingreso Consumo Eléctrico"; }
            if (codigo == "A602") { return "Salida Consumo Eléctrico"; }
            if (codigo == "A603") { return "OK"; }
            if (codigo == "A604") { return "NO OK"; }
            if (codigo == "A605") { return "Búsqueda por Consumo Eléctrico OK"; }
            if (codigo == "A606") { return "Búsqueda por Consumo Eléctrico NO OK"; }
            if (codigo == "A607") { return "Generar cupón Consumo Eléctrico"; }
            if (codigo == "A608") { return "Generar cupón Consumo Eléctrico OK"; }
            if (codigo == "A609") { return "Generar cupón Consumo Eléctrico NO OK"; }

            // Historial de Pago
            if (codigo == "A700") { return "Historial de Pago"; }
            if (codigo == "A701") { return "Ingreso Historial de Pago"; }
            if (codigo == "A702") { return "Salida Historial de Pago"; }
            if (codigo == "A703") { return "OK"; }
            if (codigo == "A704") { return "NO OK"; }
            if (codigo == "A705") { return "Búsqueda por historial de Pago OK"; }
            if (codigo == "A706") { return "Búsqueda por historial de Pago NO OK"; }
            if (codigo == "A707") { return "Generar cupón de Historial de Pago"; }
            if (codigo == "A708") { return "Generar cupón de Historial de Pago OK"; }
            if (codigo == "A709") { return "Generar cupón de Historial de Pago NO OK"; }

            // Certificado Existencia
            if (codigo == "A800") { return "Certificado de Existencia"; }
            if (codigo == "A801") { return "Búsqueda por Certificado de Existencia OK"; }
            if (codigo == "A802") { return "Búsqueda por  Certificado de Existencia NO OK"; }
            if (codigo == "A803") { return "Previsualizar  Certificado de Existencia"; }
            if (codigo == "A804") { return "Imprimir  Certificado de Existencia"; }
            if (codigo == "A805") { return "Imprimir  Certificado de Existencia OK"; }
            if (codigo == "A806") { return "Imprimir  Certificado de Existencia NO OK"; }
            if (codigo == "A807") { return "Enviar  Certificado de Existencia por Email"; }
            if (codigo == "A808") { return "Enviar  Certificado de Existencia por Email OK"; }

            // Certificado de Consumo
            if (codigo == "A900") { return "Certificado de Consumo"; }
            if (codigo == "A901") { return "Búsqueda por Certificado de Consumo OK"; }
            if (codigo == "A902") { return "Búsqueda por  Certificado de Consumo NO OK"; }
            if (codigo == "A903") { return "Previsualizar  Certificado de Consumo"; }
            if (codigo == "A904") { return "Imprimir  Certificado de Consumo"; }
            if (codigo == "A905") { return "Imprimir  Certificado de Consumo OK"; }
            if (codigo == "A906") { return "Imprimir  Certificado de Consumo NO OK"; }
            if (codigo == "A907") { return "Enviar  Certificado de Consumo por Email"; }
            if (codigo == "A908") { return "Enviar  Certificado de Consumo por Email OK"; }

            // Certificado de Cuenta Corriente
            if (codigo == "A110") { return "Certificado de Cuenta Corriente"; }
            if (codigo == "A111") { return "Búsqueda por Certificado de Cuenta Corriente OK"; }
            if (codigo == "A112") { return "Búsqueda por  Certificado de Cuenta Corriente NO OK"; }
            if (codigo == "A113") { return "Previsualizar  Certificado de Cuenta Corriente"; }
            if (codigo == "A114") { return "Imprimir  Certificado de Cuenta Corriente"; }
            if (codigo == "A115") { return "Imprimir  Certificado de Cuenta Corriente OK"; }
            if (codigo == "A116") { return "Imprimir  Certificado de Cuenta Corriente NO OK"; }
            if (codigo == "A117") { return "Enviar  Certificado de Cuenta Corriente por Email"; }
            if (codigo == "A118") { return "Enviar  Certificado de Cuenta Corriente por Email OK"; }

            else
            {
                return "";
            }
        }
    }
}