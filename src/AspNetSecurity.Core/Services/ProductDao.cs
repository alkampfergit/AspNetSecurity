using AspNetSecurity.Core.Models;
using AspNetSecurity.Core.SqlHelpers;

namespace AspNetSecurity.Core.Services
{
    public class ProductDao
    {
        private readonly DatabaseManager _databaseManager;

        public ProductDao(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        //public Product GetProductById(string productId)
        //{
        //    var query = DataAccess.CreateQuery($"Select * from dbo.Products where productId = {productId}");
        //    return query.ExecuteBuildSingleEntity(Product.Builder);
        //}

        //public Product GetProductById(int productId)
        //{
        //    var query = DataAccess.CreateQuery($"Select * from dbo.Products where productId = {productId}");
        //    return query.ExecuteBuildSingleEntity(Product.Builder);
        //}

        public Product GetProductById(string productId)
        {
            var query = DataAccess
                .CreateQueryOn(
                    _databaseManager.GetConnectionStringForUser(Constants.DatabaseUsers.ProductReader),
                    $"Select * from dbo.Products where productId = {productId}");
            return query.ExecuteBuildSingleEntity(Product.Builder);
        }
    }
}
