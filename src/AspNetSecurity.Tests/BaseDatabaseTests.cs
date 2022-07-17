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
            DataAccess.SetConnectionString(Constants.TestConnectionString);
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
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            var dbNames = DataAccess.CreateQuery("SELECT name from sys.databases").ExecuteList<string>();
            foreach (var dbName in dbNames)
            {
                if (dbName.StartsWith("TESTDB_"))
                {
                    using (DisableTransactionScope.Enter()) DataAccess.CreateQuery($"DROP DATABASE {dbName}").ExecuteNonQuery();
                }
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
        public void Can_create_user()
        {
            var userPassword = _sut.EnsureUser(_databaseName, "Frank", new[] { "MyTable"});
            Assert.IsNotNull(userPassword);
            var user = DataAccess.CreateQuery(@"select sp.name,
       sp.type_desc as login_type
      
from sys.server_principals sp
where sp.name = 'Frank'")
                .ExecuteScalar<string>();

            Assert.That(user, Is.EqualTo("Frank"));
        }
    }
}