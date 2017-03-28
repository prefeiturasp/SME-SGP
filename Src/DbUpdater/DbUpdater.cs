using DbUp;
using System;
using System.Reflection;
using DbUp.Engine;
using System.Collections.Generic;
using static System.Console;
using System.Data.SqlClient;

namespace DbUpdater
{
    internal class DbUpdater
    {
        private static int Main(string[] args)
        {
            List<DatabaseUpgradeResult> results = new List<DatabaseUpgradeResult>();

            bool log = false;

            try
            {
                string file = "dbSettings.json";

                foreach (string arg in args)
                {
                    if (arg.EndsWith(".json"))
                    {
                        file = arg;
                    }

                    if (arg.ToUpper().Equals(@"/LOG"))
                    {
                        log = true;
                    }
                }

                Config config = new Config(file);

                foreach (DatabaseConfig databaseConfig in config.DbSettings)
                {
                    databaseConfig.SystemId = config.SystemId;

                    var result = DeployDataBase(databaseConfig);

                    results.Add(result);

                    if (!result.Successful)
                    {
                        Error("Error!");
                        return -1;
                    }
                }

                Sucess("Sucess!");
                WriteConsole(Log.SaveLog(results, log), ConsoleColor.Yellow);

                return 0;
            }
            catch (Exception e)
            {
                results.Add(new DatabaseUpgradeResult(new List<SqlScript>(), false, e));
                WriteConsole(Log.SaveLog(results, log), ConsoleColor.Yellow);

                Error(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Update Performer with journal
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        public static DatabaseUpgradeResult DeployDataBase(DatabaseConfig dbConfig)
        {
            var databaseConnectionString = dbConfig.DeployConnectionString;
            var connection = new SqlConnectionStringBuilder(databaseConnectionString);
            var database = connection.InitialCatalog;
            var timeout = TimeSpan.FromSeconds(connection.ConnectTimeout);
            var assembly = Assembly.GetExecutingAssembly();

            WriteConsole($"Deploying {database}...", ConsoleColor.Yellow);

            EnsureDatabase.For.SqlDatabase(databaseConnectionString);

            Func<string, bool> filterSqlFiles = (x) => x.StartsWith(dbConfig.ProjectDatabase, StringComparison.OrdinalIgnoreCase)
                                                        && x.EndsWith(".sql", StringComparison.OrdinalIgnoreCase);

            SqlJournalSystem journal = new SqlJournalSystem(dbConfig.ConnectionFactory, dbConfig.SystemId);

            var upgradeDB =
            DeployChanges.To
                .SqlDatabase(databaseConnectionString)
                .WithScriptsEmbeddedInAssembly(assembly, x => filterSqlFiles(x))
                .WithVariables(dbConfig.Variables)
                .WithExecutionTimeout(timeout)
                .JournalTo(journal)
                .WithTransaction()
                .LogToConsole()
                .Build();

            return upgradeDB.PerformUpgrade();
        }

        private static void WriteConsole(string msg, ConsoleColor color)
        {
            ForegroundColor = color;
            WriteLine(msg);
            ResetColor();
        }

        private static void Sucess(string msg)
        {
            WriteConsole(msg, ConsoleColor.Green);
        }

        private static void Error(string msg)
        {
            WriteConsole(msg, ConsoleColor.Red);
        }
    }
}