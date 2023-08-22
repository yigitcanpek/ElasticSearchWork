using Elastic.Clients.Elasticsearch;

namespace ElasticSearchWork.API.Models
{
    public class Product
    {
        public string ID { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeUpdated { get; set; }
        public ProductFeature? ProductFeature { get; set; }

        public ProductDto CreateDto()
        {
            if (ProductFeature == null)
                return new ProductDto(ID, Name, Price, Stock, null);

            return new ProductDto(ID = ID, Name = Name, Price = Price, Stock = Stock, null);
        }
    }
}
