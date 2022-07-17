using AspNetSecurity.Core.Models;
using AspNetSecurity.Core.SqlHelpers;

namespace AspNetSecurity.Core.Services
{
    public class ProductDao
    {
        //public Product GetProductById(string productId)
        //{
        //    var query = DataAccess.CreateQuery($"Select * from dbo.Products where productId = {productId}");
        //    return query.ExecuteBuildSingleEntity(Product.Builder);
        //} 
        
        public Product GetProductById(int productId)
        {
            var query = DataAccess.CreateQuery($"Select * from dbo.Products where productId = {productId}");
            return query.ExecuteBuildSingleEntity(Product.Builder);
        }
    }
}
