using Elastic.Clients.Elasticsearch;
using ElasticSearchWork.API.Dtos;
using ElasticSearchWork.API.Models;
using System.Collections.Immutable;

namespace ElasticSearchWork.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _client;

        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }
        public async Task<Product> SaveAsync(Product newProduct)
        {
            newProduct.DateTimeCreated = DateTime.Now;

            IndexResponse? response = await _client.IndexAsync(newProduct, x => x.Index("products").Id(Guid.NewGuid().ToString()));

            if (!response.IsValidResponse)
            {
                return null;
            }
            newProduct.ID = response.Id;

            return newProduct;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            SearchResponse<Product> query = await _client.SearchAsync<Product>(s => s.Index("products").Query(q => q.MatchAll()));
            foreach (var item in query.Hits) item.Source.ID = item.Id;
            

            
            return query.Documents.ToImmutableList();

        }
        public async Task<Product> GetByIdAsync(string id)
        {

            GetResponse<Product> response = await _client.GetAsync<Product>(id, s => s.Index("products"));
            response.Source.ID = response.Id;
           
            return response.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            UpdateResponse<Product> response = await _client.UpdateAsync<Product,ProductUpdateDto>(/*updateProduct.id, x=> x.Index("products").Doc(updateProduct)*/ "products",updateProduct.id,x=> x.Doc(updateProduct));

            return response.IsValidResponse;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            DeleteResponse response = await _client.DeleteAsync<Product>(id, s => s.Index("products"));

            if (response.IsValidResponse)
                return true;

            return false;
        }
    }
}
