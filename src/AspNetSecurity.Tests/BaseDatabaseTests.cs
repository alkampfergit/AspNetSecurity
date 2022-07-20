using AspNetSecurity.Core.Helpers;
using AspNetSecurity.Core.SqlHelpers;

namespace AspNetSecurity.Tests
{
    [TestFixture]
    public class Tests
    {
        private DatabaseManager _sut;
        private string _databaseName;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _databaseName = "TESTDB_" + Guid.NewGuid().ToString().Replace("-", "");
            DataAccess.SetConnectionString(Constants.TestConnectionString.Replace("db-name-here", "master"));
            _sut = new DatabaseManager();
            _sut.CreateDatabase(_databaseName);

            DataAccess.CreateQuery(@$"USE {_databaseName}

CREATE TABLE [dbo].[MyTable](
	[CustomerID] [nchar](5) NOT NULL,
	[CompanyName] [nvarchar](40) NOT NULL,

 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]").ExecuteNonQuery();

            DataAccess.CreateQuery(@$"USE {_databaseName}

CREATE TABLE [dbo].[TransientUsers](
	[Password] [varchar](64) NOT NULL,
	[UserName] [varchar](50) NOT NULL,

 CONSTRAINT [PK_TransientUsers] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]").ExecuteNonQuery();

            DataAccess.SetConnectionString(Constants.TestConnectionString.Replace("db-name-here", _databaseName));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DataAccess.SetConnectionString(Constants.TestConnectionString.Replace("db-name-here", "master"));
            try
            {
                var dbNames = DataAccess.CreateQuery("SELECT name from sys.databases").ExecuteList<string>();
                foreach (var dbName in dbNames)
                {
                    if (dbName.StartsWith("TESTDB_"))
                    {
                        using (DisableTransactionScope.Enter()) DataAccess.CreateQuery($"USE [master]; ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {dbName}").ExecuteNonQuery();
                    }
                }

                var logins = DataAccess.CreateQuery("select sp.name as login from sys.server_principals sp").ExecuteList<string>();
                foreach (var login in logins)
                {
                    if (login.StartsWith("TESTUSER_"))
                    {
                        using (DisableTransactionScope.Enter())
                            DataAccess.CreateQuery($"USE [master]; DROP LOGIN [{login}];").ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void Can_verify_database_existing()
        {
            var exists = _sut.DatabaseExists(Guid.NewGuid().ToString());
            Assert.False(exists);
        }

        [Test]
        public void Can_create_database_then_delete()
        {
            var newDbName = "TESTDB_" + Guid.NewGuid().ToString().Replace("-", "");
            _sut.CreateDatabase(newDbName);
            Assert.True(_sut.DatabaseExists(newDbName));
        }

        [Test]
        public void Can_create_new_user()
        {
            var userName = "TESTUSER_" + Guid.NewGuid().ToString().Replace("-", "");
            var userPassword = _sut.EnsureUser(_databaseName, userName, new[] { "MyTable" });
            Assert.IsNotNull(userPassword);
            var user = DataAccess.CreateQuery(@$"select sp.name,
       sp.type_desc as login_type
      
from sys.server_principals sp
where sp.name = '{userName}'")
                .ExecuteScalar<string>();

            Assert.That(user, Is.EqualTo(userName));
        }

        [Test]
        public void Can_recover_password_of_existing_user()
        {
            var userName = "TESTUSER_" + Guid.NewGuid().ToString().Replace("-", "");
            var userPassword1 = _sut.EnsureUser(_databaseName, userName, new[] { "MyTable" });
            var userPassword2 = _sut.EnsureUser(_databaseName, userName, new[] { "MyTable" });
            Assert.IsNotNull(userPassword1);
            Assert.That(userPassword1, Is.EqualTo(userPassword2));
        }
    }
}