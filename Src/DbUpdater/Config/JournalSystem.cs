using System;
using System.Collections.Generic;
using DbUp.Engine;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace DbUpdater
{
    public class SqlJournalSystem : IJournal
    {
        private readonly Func<IDbConnection> connectionFactory;
        private readonly string tableName = "SchemaVersions";
        private readonly string schemaTableName;
        private readonly int systemId;

        /// <summary>
        /// Constructor of Journal with identifier per system
        /// </summary>
        /// <param name="connectionFactory"></param>
        /// <param name="sysId"></param>
        /// <param name="table"></param>
        /// <param name="schema"></param>
        public SqlJournalSystem(Func<IDbConnection> connectionFactory, int sysId, string table = null, string schema = null)
        {
            this.connectionFactory = connectionFactory;
            schemaTableName = (string.IsNullOrEmpty(schema) ? "" : (schema + "." )) + (string.IsNullOrEmpty(table) ? tableName : table);
            systemId = sysId > 0 ? sysId : 0;
        }

        /// <summary>
        /// Recalls the version number of the database.
        /// </summary>
        /// <returns>All executed scripts.</returns>
        public string[] GetExecutedScripts()
        {
            var exists = DoesTableExist();
            if (!exists)
            {
                return new string[0];
            }

            var scripts = new List<string>();
            using (var connection = connectionFactory())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("select [ScriptName] from {0} where [SystemId] = {1} order by [ScriptName]", schemaTableName, systemId);
                command.CommandType = CommandType.Text;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        scripts.Add((string)reader[0]);
                }
            }
            return scripts.ToArray();
        }

        /// <summary>
        /// Records a database upgrade for a database specified in a given connection string.
        /// </summary>
        /// <param name="script">The script.</param>
        public void StoreExecutedScript(SqlScript script)
        {
            var exists = DoesTableExist();
            if (!exists)
            {
                using (var connection = connectionFactory())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format(
@"create table {0} (
	[Id] int identity(1,1) not null constraint PK_SchemaVersions_Id primary key,
    [SystemId] int not null,
	[ScriptName] nvarchar(255) not null,
	[Applied] datetime not null
)", schemaTableName);

                    command.CommandType = CommandType.Text;
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }

            using (var connection = connectionFactory())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("insert into {0} (SystemId, ScriptName, Applied) values ({1}, @scriptName, '{2}')", schemaTableName, systemId, DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss"));

                var param = command.CreateParameter();
                param.ParameterName = "scriptName";
                param.Value = script.Name;
                command.Parameters.Add(param);

                command.CommandType = CommandType.Text;
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Query whether the execution control table exists
        /// </summary>
        /// <returns></returns>
        private bool DoesTableExist()
        {
            try
            {
                using (var connection = connectionFactory())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format("select count(*) from {0}", schemaTableName);
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteScalar();
                        return true;
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            catch (DbException)
            {
                return false;
            }
        }
    }
}
