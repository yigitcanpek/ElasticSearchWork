using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Elastic.Clients.Elasticsearch;

namespace ElasticSearchWork.API.Extentions.ElasticSearch
{
    public static class ElasticSearchExtention
    {
        public static void AddElastic(this IServiceCollection services,IConfiguration configuration)
        {
            //SingleNodeConnectionPool pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            //ConnectionSettings settings = new ConnectionSettings(pool);
            ////settings.BasicAuthentication(); For custom connection


            //ElasticClient client = new ElasticClient(settings);


            var userName = (configuration.GetSection("Elastic")["Username"]);
            var password = (configuration.GetSection("Elastic")["Password"]);
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!))
                .Authentication(new BasicAuthentication(userName,password));

           

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);
        }
    }
}
