using AspNetSecurity.Core.DataAccess;
using AspNetSecurity.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AspNetSecurity.Api.Controllers
{
    [Route("northwind/sqli")]
    public class SqlInjectionController : Controller
    {
        [SwaggerResponse(200, type: typeof(Product))]
        [HttpGet]
        public IActionResult GetProductVulnerable(String productId)
        {
            var query = DataAccess.CreateQuery($"Select * from dbo.Products where productId = {productId}");
            var product = query.ExecuteBuildSingleEntity(Product.Builder);
            return Ok(product);
        }
    }
}
