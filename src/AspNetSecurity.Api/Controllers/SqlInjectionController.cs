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
        [Route("products/{productId}")]
        public IActionResult GetProductVulnerable(String productId)
        {
            var query = DataAccess.CreateQuery($"Select * from dbo.Products where productId = {productId}");
            var product = query.ExecuteBuildSingleEntity(Product.Builder);
            return Ok(product);
        }

        //[SwaggerResponse(200, typeof(Customer))]
        //[HttpGet]
        //[Route("customers/{customerId}")]
        //public IActionResult GetCustomer(String customerId)
        //{
        //    var query = DataAccess.CreateQuery($"Select * from dbo.Customers where CustomerId = '{customerId}'");
        //    var customer = query.ExecuteBuildSingleEntity<Customer>(Customer.Builder);
        //    return Ok(customer);
        //}

        [SwaggerResponse(200, type: typeof(Customer))]
        [HttpGet]
        [Route("customers/{customerId}")]
        public IActionResult GetCustomer(string customerId)
        {
            var id = new CustomerId(customerId);
            var query = DataAccess.CreateQuery($"Select * from dbo.Customers where CustomerId = '{id.AsString}'");
            var customer = query.ExecuteBuildSingleEntity(Customer.Builder);
            return Ok(customer);
        }

    }
}
