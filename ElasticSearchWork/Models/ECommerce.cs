using System.Text.Json.Serialization;

namespace ElasticSearchWork.API.Models.ECommerce
{
    public class ECommerce
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("customer_first_name")]
        public string CustomerFirstName { get; set; } = null!;
        [JsonPropertyName("customer_last_name")]
        public string CustomerLastName { get; set; } = null!;
        [JsonPropertyName("customer_full_name")]
        public string CustomerFullName { get; set; } = null!;
        [JsonPropertyName("category")]
        public string[] Category { get; set; }
        [JsonPropertyName("order_id")]
        public int OrderID { get; set; }
        [JsonPropertyName("order_date")]
        public DateTime OrderDate { get; set; }
        public Product[] Products { get; set; }
        [JsonPropertyName("taxful_total_price")]
        public double TaxfulTotalPrice { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("_id")]
        public string ID { get; set; }
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
    }
}
