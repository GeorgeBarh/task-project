namespace TaskMeUp.Api.Contracts.DTO
{
    public class ApiResult<T> where T : class
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public ApiResult()
        {
           
        }

        public ApiResult(bool success,string errorCode, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
            ErrorCode = errorCode;
        }
    }
}
