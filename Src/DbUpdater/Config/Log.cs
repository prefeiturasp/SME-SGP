using System;
using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using System.IO;

namespace DbUpdater
{
    public static class Log
    {
        private static readonly string logFolder = "Log";

        public static string SaveLog(List<DatabaseUpgradeResult> results, bool saveLogFile = false)
        {
            if (!saveLogFile)
                return "";

            string logFile = "";

            try
            {
                List<string> lstLog = new List<string>();
                lstLog.Add("Date: " + DateTime.Now.ToString());

                foreach (DatabaseUpgradeResult result in results)
                {
                    lstLog.Add("Result: " + result.Successful.ToString());
                    lstLog.Add("Error Source: " + (result.Error == null ? "" : result.Error.Source));
                    lstLog.Add("Error Message: " + (result.Error == null ? "" : result.Error.Message));
                    lstLog.Add("Scripts: " + (result.Scripts.Any() ? "" : "No new scripts need to be executed"));
                    lstLog.AddRange(result.Scripts.Select(s => s.Name).ToList());
                    lstLog.Add("");
                    lstLog.Add("-------------------------------------------------------------------------------------------");
                    lstLog.Add("");
                }

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                logFile = logFolder + "\\" + "log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log";

                using (TextWriter tw = new StreamWriter(logFile))
                {
                    foreach (string s in lstLog)
                        tw.WriteLine(s);
                }
            }
            catch (Exception e)
            {
                logFile = "Error save log: " + e.Message;
            }

            return logFile;
        }
    }
}
