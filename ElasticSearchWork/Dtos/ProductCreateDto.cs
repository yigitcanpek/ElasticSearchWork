using ElasticSearchWork.API.Models;

namespace ElasticSearchWork.API.Dtos
{
    public record ProductCreateDto(string Name,decimal Price,int Stock,ProductFeatureDto Feature)
    {


        public Product CreateProduct()
        {
            return new Product { Name = Name, Price = Price, Stock = Stock, ProductFeature = new ProductFeature() { Width = Feature.Width, Color = (Color)int.Parse(Feature.color), Height = Feature.Height } };    
        }
    }
}
