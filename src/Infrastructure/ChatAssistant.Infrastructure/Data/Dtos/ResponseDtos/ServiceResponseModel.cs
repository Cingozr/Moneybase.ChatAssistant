using System.Net;

namespace ChatAssistant.Infrastructure.Data.Dtos.ResponseDtos
{
    public class ServiceResponseModel
    {
        public string Message { get; set; }
        public string AggregatedUFExceptions { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class ServiceResponseModel<T> : ServiceResponseModel where T : new()
    {
        public T Data { get; set; }
    }
}
