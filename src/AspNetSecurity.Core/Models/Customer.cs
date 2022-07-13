using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetSecurity.Core.Models
{
    public class Customer
    {
        public CustomerId CustomerId { get; set; }

        public string CompanyName { get; set; }

        public string? ContactName { get; set; }

        public string? ContactTitle { get; set; }

        public static Customer Builder(IDataRecord arg)
        {
            return new Customer()
            {
                CustomerId = new CustomerId((string) arg["CustomerID"]),
                CompanyName = (string) arg["CompanyName"],
                ContactName = arg["ContactName"] as string,
                ContactTitle = arg["ContactTitle"] as string,
            };
        }
    }

    public class CustomerId
    {
        private string Id { get; set; }

        public CustomerId(string customerId)
        {
            if (customerId.Length != 5)
                throw new ArgumentException("Invalid Id");

            if (customerId.Any(c => !char.IsLetter(c)))
                throw new ArgumentException("Invalid Id");

            Id = customerId;
        }

        public string AsString => Id;
    }

    public class CustomerIdConverter : JsonConverter<CustomerId>
    {
        public override CustomerId Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                new CustomerId(reader.GetString()!);

        public override void Write(
            Utf8JsonWriter writer,
            CustomerId customerId,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(customerId.AsString);
    }
}
