using Application.Wrappers.Abstract;
using JsonConstructorAttribute = Newtonsoft.Json.JsonConstructorAttribute;

namespace Application.Wrappers.Concrete
{
    public class DataResponse<T> : IDataResponse<T>
    {
        public bool Success { get; } = true;
        public T Data { get; }

        public int StatusCode { get; }
        private string _message;

        public string Message
        {
            get => _message ?? string.Empty;
            set => _message = value;
        }

        [JsonConstructor]
        public DataResponse(T data, int statuscode)
        {
            Data = data;
            StatusCode = statuscode;
        }

        public DataResponse(T data, int statuscode, string message)
        {
            Data = data;
            StatusCode = statuscode;
            Message = message;
        }
    }
}
