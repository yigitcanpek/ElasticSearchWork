using ElasticSearchWork.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchWork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECommerceController :BaseController
    {
        private readonly ECommerceRepository _repository;

        public ECommerceController(ECommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("TermLevelQuery")]
        public async Task<IActionResult> TermLevelQuery(string customerFirstName)
        {
            return Ok(await _repository.TermQuery(customerFirstName));
        }
        [HttpGet]
        [Route("TermsLevelQuery")]
        public async Task<IActionResult> TermsLevelQueries(List<string> customerFirstNames)
        {

            return Ok(await _repository.TermsQuery(customerFirstNames));
        }

        [HttpGet]
        [Route("PrefixQuery")]
        public async Task<IActionResult> PrefixQuery(string customerFirstNames)
        {

            return Ok(await _repository.PrefixQuery(customerFirstNames));
        }
        [HttpGet]
        [Route("RangeQuery")]
        public async Task<IActionResult> RangeQuery(double fromPrice,double toPrice)
        {

            return Ok(await _repository.RangeQuery(fromPrice,toPrice));
        }
        [HttpGet]
        [Route("MatchAllQuery")]
        public async Task<IActionResult> MatchAllQuery()
        {
            return Ok(await _repository.MatchAllQuery());
        }
        [HttpGet]
        [Route("MatchAllQueryPage")]
        public async Task<IActionResult> MatchAllQueryPage(int page , int pagesize)
        {
            return Ok(await _repository.MatchAllQueryPage(page,pagesize));
        }
        [HttpGet]
        [Route("Wildcard")]
        public async Task<IActionResult> Wildcard(string customerFullName)
        {
            return Ok(await _repository.Wildcard(customerFullName));
        }
        [HttpGet]
        [Route("Fuzzy")]
        public async Task<IActionResult> Fuzzy(string customerName)
        {
            return Ok(await _repository.Fuzzy(customerName));
        }
        [HttpGet]
        [Route("MatchFullText")]
        public async Task<IActionResult> MatchFullText(string categoryName)
        {
            return Ok(await _repository.MatchFullText(categoryName));
        }

        [HttpGet]
        [Route("MatchBoolPrefixFullText")]
        public async Task<IActionResult> MatchBoolPrefixFullText(string customerFullName) 
        {
            return Ok(await _repository.MatchBoolPrefixFullText(customerFullName));
        }

        [HttpGet]
        [Route("MatchBoolPhraseFullText")]
        public async Task<IActionResult> MatchBoolPhraseFullText(string customerFullName)
        {
            return Ok(await _repository.MatchBoolPhraseFullText(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryFirst(string geoipCityName, double taxFulTotalPrice, string manufacturer , string categoryName)
        {
            return Ok(await _repository.CompoundQueryfrst(geoipCityName, taxFulTotalPrice, manufacturer, categoryName));
        }
    }
}
