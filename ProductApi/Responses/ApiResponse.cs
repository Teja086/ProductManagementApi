namespace ProductApi.Responses
{
public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int statusCode, string message, T? data = default)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }

    public class ApiPagedResponse<T> : ApiResponse<List<T>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public ApiPagedResponse(int statusCode, string message, List<T> data, int page, int pageSize, int totalCount, int totalPages)
            : base(statusCode, message, data)
        {
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }
    }
}
