using AspNetSecurity.Core.Helpers;

namespace AspNetSecurity.Core.SqlHelpers
{
    public class DatabaseManager
    {
        private Dictionary<string, string> _connections  = new Dictionary<string, string>();

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

        public bool LoginExists(string userName)
        {
            return DataAccess
               .CreateQuery("select sp.name as login from sys.server_principals sp where sp.name = {userName}")
               .SetStringParam("userName", userName)
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
            //check if the user exists
            var loginExists = this.LoginExists(userName);
            string password = Guid.NewGuid().ToString();
            if (!loginExists)
            {
                DataAccess
                    .CreateQuery("INSERT INTO [dbo].[TransientUsers] ([UserName],[Password]) VALUES ({userName}, {pwd})")
                    .SetStringParam("userName", userName)
                    .SetStringParam("pwd", password)
                    .ExecuteNonQuery();

                DataAccess
                   .CreateQuery(@$"use {databaseName}
                        CREATE LOGIN {userName} WITH PASSWORD = '{password}'")
                   .ExecuteNonQuery();
                DataAccess
                   .CreateQuery(@$"use {databaseName}
                    CREATE USER {userName} FOR LOGIN {userName};")
                   .ExecuteNonQuery();
                foreach (var table in readonlyTables)
                {
                    DataAccess.CreateQuery($"use {databaseName}; GRANT SELECT ON {table} to [{userName}]").ExecuteNonQuery();
                }
            }
            else
            {
                // we need to grab the password from the transient user
                password = DataAccess
                    .CreateQuery("Select Password from [dbo].[TransientUsers] where username = {username}")
                        .SetStringParam("username", userName)
                        .ExecuteScalar<string>();
            }

            _connections[userName] = DataAccess.ConnectionString.Replace("Integrated Security=SSPI", $"user={userName};password={password}");

            return password;
        }

        public string GetConnectionStringForUser(string userName) 
        {
            return _connections[userName];
        }
    }
}
