using Microsoft.VisualBasic;
using SAESA.AutoAtencion.Framework.Configuracion;
using SAESA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;


namespace SAESA.Utils
{

    public class UseReportService
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public struct Results
        {
            public int CodigoError;
            public List<string> Data;
        }


        private string mensaje;
        public int Timeout { get; set; }
        private string _Resp;
        public Results Resultado;

        private Configuracion oConfig = Configuracion.Instancia();

        public UseReportService()
        {
            this.Timeout = 30 * 1000;
        }


        public void sendData(ReportModel data)
        {
            try
            {
                List<string> lData = new List<string>();
                //seguir este formato:
                // 0,IdUsr 1,"REG" 2,IdUpd 3,FHini 4,FHfin 5,Tipo 6,Detalle 7,Monto 8,fCierre 9,SubTipo 10,RutCli 11,Folio 12,CodLugar 13,RutConvenio 14,RutMedico 15,CodEspecialidad
                // 16,FHagenda 17,RutCajero 18,TipoPago 19,IdCita 20,CodPrestacion 21,NomAsegurador 22,Turno 23,? 24,?

                // IdKiosko
                lData.Add(oConfig.IdKiosko);//idUsr // IdKiosko

                lData.Add("REG");//REG
                lData.Add("0");//idUpd

                // Fecha Inicio 
                lData.Add(data.HoraInicio);//FHini

                // Fecha Termino
                lData.Add(data.HoraTermino);//FHfin

                // Resultado (Cierre)
                lData.Add(data.Modulo);//Tipo

                //
                lData.Add(data.Accion);//Detalle
                lData.Add("0");//monto
                //lData.Add(data.OperacionRealizada);//fcierre
                lData.Add("0");//fcierre

                // Modulo
                lData.Add(data.Resultado);//SubTipo 


                lData.Add(data.Rut);//RutCli
                lData.Add("0");//folio
                lData.Add("0");//codLugar
                lData.Add("S");//rutConvenio 
                lData.Add("1");//rutMedico

                // Servicio
                lData.Add(data.NroServicio);//codEspecialidad
                lData.Add(data.HoraInicio);//FHagenda
                lData.Add("1");//RutCajero
                lData.Add("0");//TipoPago
                lData.Add("0");//IdCita
                lData.Add("0");//CodPrestacion
                lData.Add(data.Session);//NomAsegurador
                lData.Add("0");//Turno
                lData.Add(data.Empresa);//23 ?
                lData.Add("");//24 ?

                mensaje = BuildMessage(lData);

                if (sendMensaje())
                {
                    ServicioGlobal trx = new ServicioGlobal();

                    if (_Resp.Length < (int)ServicioGlobal.Posicion.MIN_MSG_LEN)
                        Resultado.CodigoError = (int)ServicioGlobal.ValidaMsg.MSGLEN;
                    else if (_Resp.Length > (int)ServicioGlobal.Posicion.MAX_MSG_LEN)
                        Resultado.CodigoError = (int)ServicioGlobal.ValidaMsg.MSGLEN;
                    else if (_Resp.Substring(0, (int)ServicioGlobal.Posicion.HEAD_LEN) != ServicioGlobal.Header || _Resp.Last() != '@')
                        Resultado.CodigoError = (int)ServicioGlobal.ValidaMsg.MSGFMT;

                    if (Resultado.CodigoError != 0)
                    {
                        Resultado.Data = new List<string> { trx.GetError(Resultado.CodigoError) };
                        return;
                    }

                    int respCmd = Convert.ToInt32(_Resp.Substring((int)ServicioGlobal.Posicion.CMD_POS, (int)ServicioGlobal.Posicion.CMD_LEN));
                    int len = Convert.ToInt32(_Resp.Substring((int)ServicioGlobal.Posicion.LEN_POS, (int)ServicioGlobal.Posicion.LEN_LEN));

                    if (_Resp.Length != len)
                        Resultado.CodigoError = (int)ServicioGlobal.ValidaMsg.MSGLEN;
                    else if (respCmd != 999 && respCmd != 204)
                        Resultado.CodigoError = (int)ServicioGlobal.ValidaMsg.MSGCMD;

                    if (Resultado.CodigoError != 0)
                    {
                        Resultado.Data = new List<string> { trx.GetError(Resultado.CodigoError) };
                        return;
                    }
                    if (!ServicioGlobal.IsChecksumValid(_Resp))
                    {
                        Resultado.CodigoError = (int)ServicioGlobal.ValidaMsg.MSGCKS;
                        Resultado.Data = new List<string> { trx.GetError(Resultado.CodigoError) };
                        return;
                    }

                    string msg = _Resp.Substring((int)ServicioGlobal.Posicion.DAT_POS, len - (int)ServicioGlobal.Posicion.FIX_LEN);
                    if (respCmd == 999)
                    {
                        Resultado.CodigoError = Convert.ToInt32(msg.Substring(0, 5));
                        if (Resultado.CodigoError == (int)ServicioGlobal.ValidaMsgServer.ERRTBK)
                            Resultado.Data = new List<string> { msg.Substring(5, msg.Length - 5) };
                        else
                            Resultado.Data = new List<string> { trx.GetError(Resultado.CodigoError) };
                        return;
                    }
                    Resultado.CodigoError = 0;
                    Resultado.Data = msg.Split('|').ToList();
                }
                else
                {
                    Resultado.CodigoError = -1;
                    Resultado.Data = new List<string> { _Resp };
                }
            }
            catch (Exception)
            {
                // Log.Write("SendMessage", e.StackTrace.ToString());
            }
            return;
        }

        public bool sendMensaje()
        {
            try
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "totalpago", PipeDirection.InOut, PipeOptions.None))
                {
                    pipeClient.Connect(this.Timeout);
                    byte[] buffer = Encoding.ASCII.GetBytes(this.mensaje);
                    pipeClient.Write(buffer, 0, buffer.Length);
                    pipeClient.WaitForPipeDrain();

                    StreamReader stream = new StreamReader(pipeClient);
                    _Resp = stream.ReadToEnd();
                    //string aux = _Resp.Substring(1, _Resp.LastIndexOf("@") + 1);
                    //_Resp = aux;
                }
                log.Info("Uso,mensaje enviado, respuesta: " + _Resp);
                return true;
            }
            catch (Exception ex)
            {
                log.Info("Uso,mensaje, excepcion: " + ex.ToString());
                _Resp = ex.ToString();
                return false;
            }
        }

        public string BuildMessage(List<string> data)
        {
            string datas = string.Join("|", data);
            string header = "TP10PP";
            string trans = "204";
            datas = NormaliseString(datas);
            int length = header.Length + 5 + trans.Length + datas.Length + 4 + 1;

            return header + length.ToString().PadLeft(5, '0') + trans + datas + Checksum(header + length.ToString().PadLeft(5, '0') + trans + datas) + "@";

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

        private string NormaliseString(string text)
        {
            string resp;
            resp = text.Replace("á", "a");
            resp = resp.Replace("é", "e");
            resp = resp.Replace("í", "i");
            resp = resp.Replace("ó", "o");
            resp = resp.Replace("ú", "u");
            resp = resp.Replace("ñ", "n");
            resp = resp.Replace("Á", "A");
            resp = resp.Replace("É", "E");
            resp = resp.Replace("Í", "I");
            resp = resp.Replace("Ó", "O");
            resp = resp.Replace("Ú", "U");
            resp = resp.Replace("Ñ", "N");
            return resp;
        }
    }
}