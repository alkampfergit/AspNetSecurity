using AspNetSecurity.Core.Helpers;
using AspNetSecurity.Core.Models;

namespace AspNetSecurity.Core.SqlHelpers
{
    public class DatabaseManager
    {
        public void CreateDatabase(string databaseName)
        {
            using (DisableTransactionScope.Enter())
                DataAccess
                  .CreateQuery($"create database [{databaseName}]")
                  .ExecuteNonQuery();
        }

        public bool DatabaseExists(string databaseName)
        {
            return DataAccess
                .CreateQuery("select name from sys.databases where name = {databaseName}")
                .SetStringParam("databaseName", databaseName)
                .ExecuteList<string>().Count > 0;
        }

        /// <summary>
        /// Ensure that a user is present on a datbase with read permission to certain
        /// list of tables
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="userName"></param>
        /// <param name="readonlyTables"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string EnsureUser(string databaseName, string userName, string[] readonlyTables)
        {
            string password = Guid.NewGuid().ToString();
            DataAccess
                .CreateQuery(@$"use {databaseName}
CREATE LOGIN {userName} WITH PASSWORD = '{password}';
CREATE USER {userName} FOR LOGIN {userName}; 
").ExecuteNonQuery();

            foreach (var table in readonlyTables)
            {
                DataAccess.CreateQuery($"GRANT SELECT ON {table} to [{userName}]").ExecuteNonQuery();
            }

            return password;
        }
    }
}
