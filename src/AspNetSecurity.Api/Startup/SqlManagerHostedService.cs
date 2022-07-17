using AspNetSecurity.Core.SqlHelpers;

namespace AspNetSecurity.Api.Startup
{
    public class SqlManagerHostedService : IHostedService
    {
        private readonly DatabaseManager _databaseManager;

        public SqlManagerHostedService(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _databaseManager.EnsureUser("Northwind", Constants.DatabaseUsers.ProductReader, new[] { "Products"});
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
