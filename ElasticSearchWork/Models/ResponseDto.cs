using Elastic.Clients.Elasticsearch;
using System.Net;

namespace ElasticSearchWork.API.Models
{
    public record ResponseDto<T>
    {
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        //Static Factory method
        public static ResponseDto<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode };
        }

        public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode status)
        {
            return new ResponseDto<T> { Errors = errors, StatusCode = status };
        }
        public static ResponseDto<T> Fail(string error, HttpStatusCode status)
        {
            return new ResponseDto<T> { Errors = new List<string> {error}, StatusCode = status };
        }
    }
}
