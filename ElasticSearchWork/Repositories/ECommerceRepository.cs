using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearchWork.API.Models.ECommerce;
using Microsoft.VisualBasic;
using System.Collections.Immutable;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.Json;

namespace ElasticSearchWork.API.Repositories
{
    public class ECommerceRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "kibana_sample_data_ecommerce";
        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }
        public async Task<ImmutableList<ECommerce>> TermQuery(string customerFirstName)
        {
            //1. Way
            var result =await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));
            foreach (Hit<ECommerce> item in result.Hits)
            {
                item.Source.Id = item.Id;
            }

            //2. Way
            SearchResponse<ECommerce> results = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Query(q => q.Term(t => t.CustomerFirstName.Suffix("keyword"),customerFirstName)));

            //3. way
            TermQuery termQuery = new TermQuery("customer_first_name.keyword") { Value = customerFirstName,CaseInsensitive = true};
            SearchResponse<ECommerce> resultsthird = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Query(termQuery));

            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            foreach (string customerFirstName in customerFirstNameList)
            {
                terms.Add(customerFirstName);
            }

            //1st Way
            //TermsQuery termsQuery = new TermsQuery()
            //{
            //    Field = "customer_first_name.keyword",
            //    Terms = new TermsQueryField(terms.AsReadOnly()),
            //};


            //SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s=> s.Query(termsQuery));

            //foreach (var item in result.Hits)
            //{
            //    item.Source.ID = item.Id;
            //}
            //return result.Documents.ToImmutableList();

            //2nd Way
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Size(100).
            Query(x => x.
            Terms(t => t.
            Field(f=> f.CustomerFirstName.Suffix("keyword")).Terms(new TermsQueryField(terms.AsReadOnly())))));

            return result.Documents.ToImmutableList();
        }
        
        public async Task<ImmutableList<ECommerce>> PrefixQuery(string CustomerFullName)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Size(100).
            Query(q=> q.
            Prefix(p=> p.
            Field(f=> f.CustomerFirstName.Suffix("keyword")).
            Value(CustomerFullName))));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQuery(double fromPrice , double toPrice)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Size(100).
            Query(q => q.
            Range(r => r.
            NumberRange(nr => nr.
            Field(f=> f.TaxfulTotalPrice).Gte(fromPrice).Lte(toPrice)))));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllQuery()
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Size(100).
            Query(q=> q.
            MatchAll()));

            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MatchAllQueryPage(int page, int pagesize)
        {
            Int32 pageFrom = (page- 1) * pagesize;  
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
            Size(page * pagesize).
            From(pageFrom));
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> Wildcard(string customerFullName)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s=> s.Index(indexName).
            Query(q=> q.
            Wildcard(w=> w.
            Field(f=> f.CustomerFullName.
            Suffix("keyword")).
            Wildcard(customerFullName))));

            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> Fuzzy(string customerName)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Query(q=> q.
            Fuzzy(fu=> fu.
            Field(fi=> fi.CustomerFirstName.
            Suffix("keyword")).
            Value(customerName).
            Fuzziness(new Fuzziness(1))) /* fuzzines value */).
            Sort(sort=> sort.
            Field(fie=> fie.TaxfulTotalPrice,new FieldSort() { Order = SortOrder.Desc})));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchFullText(string categoryName)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Size(1000).
            Query(q=> q.
            Match(m=> m.
            Field(fi=> fi.Category).
            Query(categoryName)/*.
            Operator(Operator.And)*/)));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullText(string customerFullName)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Size(1000).
            Query(q => q.
            MatchBoolPrefix(m => m.
            Field(fi => fi.CustomerFullName).
            Query(customerFullName)/*.
            Operator(Operator.And)*/)));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchBoolPhraseFullText(string customerFullName)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Size(1000).
            Query(q => q.
            MatchPhrase(m => m.
            Field(fi => fi.CustomerFullName).
            Query(customerFullName)/*.
            Operator(Operator.And)*/)));
            return result.Documents.ToImmutableList();
        }


        public async Task<ImmutableList<ECommerce>> CompoundQueryfrst(string geoipCityName,double taxFulTotalPrice,string manufacturer,string categoryName)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            Size(1000).
            Query(q => q.
                Bool(b => b.



                    Must(m => m.
                        Term(t => t.
                            Field("geoip.city_name").
                                Value(geoipCityName))).
                    MustNot(mn=> mn.
                        Range(r=> r.
                            NumberRange(nr=> nr.
                                Field(f=> f.TaxfulTotalPrice).
                                    Lte(taxFulTotalPrice)))).
                    Should(s=> s.
                        Term(t=> t.
                            Field(f=> f.Category.
                                Suffix("keyword")).Value(categoryName))).
                    Filter(f=> f.
                            Term(t=> t.
                                Field("manufacturer.keyword").
                                        Value(manufacturer)))
            )));
            
            return result.Documents.ToImmutableList();
        }


        public async Task<ImmutableList<ECommerce>> CompoundQueryscnd(string customerFullName)
        {


            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName)
            .Size(1000).
            Query(q => q.
                    MatchPhrase(m=> m.
                        Field(f => f.CustomerFullName).
                            Query(customerFullName))));


            /* BAD WAY */
            //SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).
            //Size(1000).
            //Query(q => q.
            //    Bool(b => b.



            //        Should(m => m.
            //            Match(ma => ma.
            //                Field(f => f.CustomerFullName).
            //                    Query(customerFullName)).
            //                        Prefix(p=> p.
            //                            Field(f=> f.CustomerFullName).
            //                                Value(customerFullName))))));


            return result.Documents.ToImmutableList();

        }

        public async Task<ImmutableList<ECommerce>> MultiMatchQuery(string name)
        {
            SearchResponse<ECommerce> result = await _client.SearchAsync<ECommerce>(s => s.
            Index(indexName).
                Size(1000).
                    Query(q => q.
                        MultiMatch(mm => mm.
                            Fields(new Field("customer_first_name").
                                And(new Field("customer_last_name")).
                                And(new Field("customer_full_name"))).
                                    Query(name))));
            return result.Documents.ToImmutableList();
        }
    }
}
