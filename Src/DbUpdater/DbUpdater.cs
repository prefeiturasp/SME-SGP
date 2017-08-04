using DbUp;
using System;
using System.Reflection;
using DbUp.Engine;
using System.Collections.Generic;
using DbUpdater.Core.Config;
using DbUpdater.Core.SqlHelpers;

namespace DbUpdater
{
    internal class DbUpdater
    {
        /// <summary>
        /// Update Performer with journal
        /// </summary>
        /// <param name="dbConfig">
        /// </param>
        /// <returns>
        /// </returns>
        public static DatabaseUpgradeResult DeployDataBase(DatabasesSettings dbConfig)
        {
            var assembly = Assembly.Load(dbConfig.ProjectDatabase);

            ConsoleOutput.Warning($"Deploying {dbConfig.InitialCatalog}...");
            Func<string, bool> filterSqlFiles = (x) => x.StartsWith(dbConfig.ProjectDatabase, StringComparison.OrdinalIgnoreCase)
                                                        && x.EndsWith(".sql", StringComparison.OrdinalIgnoreCase);

            SqlJournalSystem journal = new SqlJournalSystem(dbConfig.ConnectionFactory, dbConfig.SystemId);

            var upgradeDB =
            DeployChanges.To
                .SqlDatabase(dbConfig.DeployConnectionString)
                .WithScriptsEmbeddedInAssembly(assembly, x => filterSqlFiles(x))
                .WithVariables(dbConfig.Variables)
                .WithExecutionTimeout(TimeSpan.FromSeconds(dbConfig.ConnectTimeout))
                .JournalTo(journal)
                .WithTransaction()
                .LogToConsole()
                .Build();

            return upgradeDB.PerformUpgrade();
        }

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

                var config = Config.CreateFromFile(file);
                Config.Validate(config);

                foreach (var dbSettings in config.DbSettings)
                {
                    dbSettings.SystemId = config.SystemId;

                    if (!new DbRestore(dbSettings).RestoreIfNotExist())
                    {
                        throw new Exception("Error Restore!");
                    }

                    var result = DeployDataBase(dbSettings);
                    results.Add(result);

                    if (!result.Successful)
                    {
                        ConsoleOutput.Error("Error!");
                        return -1;
                    }
                }

                ConsoleOutput.Sucess("Sucess!");
                ConsoleOutput.Warning(Log.SaveLog(results, log));
                return 0;
            }
            catch (Exception e)
            {
                results.Add(new DatabaseUpgradeResult(new List<SqlScript>(), false, e));
                ConsoleOutput.Warning(Log.SaveLog(results, log));
                ConsoleOutput.Error(e.Message);
                return -1;
            }
        }
    }
}