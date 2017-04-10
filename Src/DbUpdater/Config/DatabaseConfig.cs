using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System;
using System.IO;

namespace DbUpdater
{
    public interface ISystem
    {
        int SystemId { get; set; }
    }

    public class Config : ISystem
    {
        public int SystemId { get; set; }
        public List<DatabaseConfig> DbSettings { get; set; }

        /// <summary>
        /// Loads the settings from the file (.json)
        /// </summary>
        /// <param name="jsonfile"></param>
        /// <returns></returns>
        public Config(string configFile)
        {
            if (File.Exists(configFile))
            {
                Config config = GetFromJsonFile(configFile);

                SystemId = config.SystemId;
                DbSettings = config.DbSettings;
            }
        }

        /// <summary>
        /// Loads the settings from the file (.json)
        /// </summary>
        /// <param name="jsonfile"></param>
        /// <returns></returns>
        private static Config GetFromJsonFile(string jsonFile)
        {
            var settings = JsonConvert.DeserializeObject<Config>(File.ReadAllText(jsonFile));
            return settings;
        }

    }

    /// <summary>
    /// Class with connection parameters and updater execution
    /// </summary>
    public class DatabaseConfig : ISystem
    {
        public int SystemId { get; set; }
        private Dictionary<string, string> variables = new Dictionary<string, string>();
        public string ProjectDatabase { get; set; }
        public string DeployConnectionString { get; set; }
        public Dictionary<string, string> Variables
        {
            get
            {
                return this.variables;
            }
            set
            {
                this.variables = value;
            }
        }

        [JsonIgnore]
        public Func<IDbConnection> ConnectionFactory
        {
            get
            {
                return BuildConnectionFactory();
            }
        }

        private Func<IDbConnection> BuildConnectionFactory()
        {
            return new Func<IDbConnection>(() =>
            {
                var conn = new SqlConnection(DeployConnectionString);
                return conn;
            });
        }

    }
}