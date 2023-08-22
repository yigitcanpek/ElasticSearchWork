using ElasticSearchWork.API.Models;

namespace ElasticSearchWork.API.Dtos
{
    public record ProductUpdateDto(string id,string Name, decimal Price, int Stock, ProductFeatureDto Feature)
    {
    }
}
