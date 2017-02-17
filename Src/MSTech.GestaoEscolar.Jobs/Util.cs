using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MSTech.CoreSSO.Entities;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.Jobs.Schedulers;
using Quartz;

namespace MSTech.GestaoEscolar.Jobs
{
    public class Util
    {
        private static string GetErrorMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendFormat("** {0} **\r\n", DateTime.Now);
                sb.AppendFormat("Exception Type: {0}\r\n", ex.GetType());

                sb.AppendFormat("Exception: {0}\r\n", ex.Message);
                sb.AppendFormat("Source: {0}\r\n", ex.Source);

                if (ex.StackTrace != null)
                {
                    sb.AppendFormat("Stack Trace: {0}\r\n\r\n", ex.StackTrace);
                }

                Exception exTemp = null;
                if (ex.InnerException != null)
                    exTemp = ex.InnerException;
                while (exTemp != null)
                {
                    sb.AppendFormat("Inner Exception Type: {0}\r\n", exTemp.GetType());
                    sb.AppendFormat("Inner Exception: {0}\r\n", exTemp.Message);
                    sb.AppendFormat("Inner Source: {0}\r\n", exTemp.Source);
                    if (exTemp.StackTrace != null)
                    {
                        sb.AppendFormat("Inner Stack Trace: {0}\r\n\r\n", exTemp.StackTrace);
                    }
                    exTemp = exTemp.InnerException;
                }
            }
            catch
            {
            }
            return sb.ToString();
        }

        public static bool GravarErro(Exception ex, SchedulerContext context) 
        {
            int sis_id = -1;
            try
            {
                sis_id = context.Contains("sis_id") ? context.GetInt("sis_id") : -1;
            }
            catch { }   
            return GravarErro(ex, sis_id);            
        }

        public static bool GravarErro(Exception ex, int sis_id)
        {
            bool ret = false;
            try
            {
                LOG_Erros entity = new LOG_Erros();
                entity.sis_id = sis_id;
                entity.err_descricao = GetErrorMessage(ex);
                entity.err_erroBase = ex.GetBaseException().Message;
                entity.err_tipoErro = ex.GetBaseException().GetType().FullName;
                entity.err_dataHora = DateTime.Now;
                entity.err_machineName = Environment.MachineName;

                string strHostName;
                string clientIPAddress = "";
                try
                {
                    strHostName = System.Net.Dns.GetHostName();
                    clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();
                }
                catch
                {

                }
                entity.err_ip = String.IsNullOrEmpty(clientIPAddress) ? "0.0.0.0" : clientIPAddress;

                LOG_ErrosBO.Save(entity);
            }
            catch { }

            return ret;
        }
    }
}
