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

            Func<string, bool> filterSqlFiles = dbConfig.GetFilterSqlFiles();

            SqlJournalSystem journal = new SqlJournalSystem(dbConfig);

            var upgradeDB =
            DeployChanges.To
                .SqlDatabase(dbConfig.DeployConnectionString)
                .WithScriptsEmbeddedInAssembly(assembly, x => filterSqlFiles(x))
                .WithVariables(dbConfig.Variables)
                .WithExecutionTimeout(dbConfig.ConnectTimeout)
                .JournalTo(journal)
                .WithTransaction()
                .LogToConsole()
                .Build();

            return upgradeDB.PerformUpgrade();
        }

        private static int Main(string[] args)
        {
            List<DatabaseUpgradeResult> results = new List<DatabaseUpgradeResult>();

            var options = Argument.Load(args);

            try
            {
                var config = Config.CreateFromFile(options.SettingFile);
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
                        throw result.Error;
                    }
                }

                ConsoleOutput.Sucess("Sucess!");
                ConsoleOutput.Warning(Log.SaveLog(results, options.Log));
                return 0;
            }
            catch (Exception e)
            {
                results.Add(new DatabaseUpgradeResult(new List<SqlScript>(), false, e));
                ConsoleOutput.Warning(Log.SaveLog(results, options.Log));
                ConsoleOutput.Error(e.Message);
                Log.Error(e);
                return 2;
            }
        }
    }
}