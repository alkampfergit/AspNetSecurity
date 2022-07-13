using System.Data;

namespace AspNetSecurity.Core.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }

        public string CompanyName { get; set; }

        public string? ContactName { get; set; }

        public string? ContactTitle { get; set; }

        public static Customer Builder(IDataRecord arg)
        {
            return new Customer()
            {
                CustomerId = (string) arg["CustomerID"],
                CompanyName = (string) arg["CompanyName"],
                ContactName = arg["ContactName"] as string,
                ContactTitle = arg["ContactTitle"] as string,
            };
        }
    }

    //[JsonConverter(typeof(CustomerIdClassConverter))]
    //public class CustomerId
    //{
    //    private string _id;
    //    public string Id => _id;

    //    public CustomerId(string customerId)
    //    {
    //        if (customerId.Length != 5)
    //            throw new ArgumentException("Invalid Id");

    //        if (customerId.Any(c => !char.IsLetter(c)))
    //            throw new ArgumentException("Invalid Id");

    //        _id = customerId;
    //    }
    //}

    //public class CustomerIdClassConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        if (objectType == typeof(CustomerId))
    //            return true;

    //        return false;
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.Value is string id)
    //        {
    //            return new CustomerId(id);
    //        }
    //        reader.Read();
    //        if ("id".Equals(reader.Value as string, StringComparison.OrdinalIgnoreCase))
    //        {
    //            //we have an id property, just read it
    //            reader.Read();
    //            return new CustomerId(reader.Value as string);
    //        }

    //        throw new ArgumentException("Value is not a valid id");
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        if (value is CustomerId customerId)
    //        {
    //            var o = JToken.FromObject(customerId.Id);
    //            o.WriteTo(writer);
    //            return;
    //        }
    //        throw new ArgumentException("Value is not a valid id");
    //    }
    //}
}
