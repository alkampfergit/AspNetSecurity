using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AspNetSecurity.Core.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        public bool Discontinued { get; set; }

        public static Product Builder(IDataRecord arg)
        {
            return new Product()
            {
                ProductId = (int)arg["ProductID"],
                ProductName = arg["ProductName"] as string,
                SupplierId = (int)arg["SupplierId"],
                CategoryId = (int)arg["CategoryId"],
                Discontinued = (bool)arg["Discontinued"],
            };
        }
    }
}
