
// 52-1 Exception class to handle Exceptions.
namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }
        // Details will contains the stacktrace.
        public string Details { get; set; }
    }
}