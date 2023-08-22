using Elastic.Clients.Elasticsearch;
using ElasticSearchWork.API.Dtos;

namespace ElasticSearchWork.API.Models
{
    public record ProductDto(string ID, string Name, decimal Price, int Stock, ProductFeatureDto? ProductFeature)
    {
    }
}
